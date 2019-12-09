using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;
using Nodegem.Services.Data;

namespace Nodegem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly ILogger<OAuthController> _logger;

        public OAuthController(SignInManager<ApplicationUser> signinManager, IUserService userService,
            ILogger<OAuthController> logger)
        {
            _signinManager = signinManager;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("external-login")]
        public ActionResult ExternalLoginAsync(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "OAuth", new {returnUrl});
            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet("link-external-account/{id:guid}")]
        public ActionResult LinkExternalLoginAsync(Guid id, string provider, string returnUrl)
        {
            var redirectUrl = Url.Action(nameof(LinkExternalCallback), "OAuth", new {userId = id, returnUrl});
            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [HttpGet("link-external-callback")]
        public async Task<ActionResult> LinkExternalCallback(string userId = null, string returnUrl = null,
            string remoteError = null)
        {
            var info = await _signinManager.GetExternalLoginInfoAsync();

            try
            {
                await _userService.LinkLoginInfo(Guid.Parse(userId), info);
                return Redirect(
                    $"{returnUrl}?success={true.ToString().ToLower()}&provider={info.LoginProvider}");
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                _logger.LogError(ex, "Unable to link account");
                return Redirect(
                    $"{returnUrl}?success={false.ToString().ToLower()}&message={message}&provider={info.LoginProvider}");
            }
        }

        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            var success = false;
            var info = await _signinManager.GetExternalLoginInfoAsync();
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                var user = await _userService.GetByLoginInfoAsync(info);
                var token = await _userService.GetTokenAsync(user.Adapt<ApplicationUser>());
                success = true;
                return Redirect($"{returnUrl}?token={token.AccessToken}&success={success.ToString().ToLower()}");
            }

            var message = $"Unable to link account to {info.LoginProvider}";
            var name = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var surname = info.Principal.FindFirstValue(ClaimTypes.Surname);
            var avatarUrl = info.Principal.HasClaim(c => c.Type == "urn:google:picture")
                ? info.Principal.FindFirst("urn:google:picture").Value
                : "";
            try
            {
                if (await _userService.UserExistsAsync(email))
                {
                    message =
                        $"Looks like your account was created through our site. You can associate your {info.LoginProvider} account to this one if you access your profile settings.";
                }
                else
                {
                    var token = await _userService.RegisterAsync(new RegisterDto
                    {
                        UserName = email,
                        Email = email,
                        Password = _userService.GeneratePassword(),
                        FirstName = name,
                        LastName = surname,
                        AvatarUrl = avatarUrl
                    }, info);
                    success = true;
                    message = "Successfully created account!";
                    return Redirect(
                        $"{returnUrl}?token={token.AccessToken}&message={message}&success={success.ToString().ToLower()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to register new external user. Provider: {info.LoginProvider}");
            }

            return Redirect(
                $"{returnUrl}?token=false&message={message}&success={success.ToString().ToLower()}");
        }
    }
}