using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nodester.Data.Dto.UserDtos;
using Nodester.Data.Models;
using Nodester.Services.Data;

namespace Nodester.WebApi.Controllers
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
        public ActionResult LoginGoogleAsync(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "OAuth", new {returnUrl});
            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
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
                var user = await _userService.GetUserByEmailAsync(email);
                var token = _userService.GetToken(user);
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