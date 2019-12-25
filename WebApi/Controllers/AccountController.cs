using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;
using Nodegem.Data.Dto;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Services.Data;
using Nodegem.Services.Exceptions;
using Nodegem.Services.Exceptions.Login;
using Nodegem.Services.Extensions;
using Nodegem.Services.Hubs;
using Nodegem.WebApi.Extensions;

namespace Nodegem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IHubContext<GraphHub> _graphHubContext;
        private readonly IDistributedCache _distributedCache;

        public AccountController(IUserService userService, ILogger<AccountController> logger,
            IHubContext<GraphHub> graphHubContext, IDistributedCache distributedCache)
        {
            _userService = userService;
            _logger = logger;
            _graphHubContext = graphHubContext;
            _distributedCache = distributedCache;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(IEnumerable<IdentityError>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenDto>> RegisterAsync([FromBody] RegisterDto dto)
        {
            try
            {
                return await _userService.RegisterAsync(dto);
            }
            catch (RegistrationException ex)
            {
                _logger.LogError(ex, "Unable to register new account");
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                const string message = "Something went wrong during registration";
                _logger.LogError(ex, message);
                return StatusCode(500, message);
            }
        }

        [AllowAnonymous]
        [HttpGet("login")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenDto>> LoginAsync()
        {
            var username = "";
            try
            {
                var (u, password) = Request.GetAuthorization();
                username = u;
                return await _userService.LoginAsync(username, password);
            }
            catch (NoUserFoundException ex)
            {
                _logger.LogError(ex, $"No user found with username: {username}");
                return BadRequest(ex.Message);
            }
            catch (InvalidLoginCredentialException)
            {
                _logger.LogError($"Invalid credentials. Username: {username}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Invalid credentials. Username: {username}");
                return BadRequest("Something went wrong.");
            }
        }
        
        [HttpGet("constants")]
        public async Task<ActionResult<IEnumerable<Constant>>> GetUserConstantsAsync()
        {
            var userId = User.GetUserId();
            try
            {
                return Ok(await _userService.GetConstantsAsync(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to get user's constants. User ID: {userId}");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("refreshToken/{oldToken}")]
        public ActionResult<TokenDto> RefreshToken(string oldToken)
        {
            try
            {
                return _userService.RefreshToken(oldToken);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to refresh token. Old Token: {oldToken}");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("email-confirmation")]
        public async Task<IActionResult> EmailConfirmationAsync([FromQuery(Name = "userId")] Guid userId,
            [FromQuery(Name = "token")] string token)
        {
            try
            {
                var result = await _userService.ConfirmEmailAsync(userId, HttpUtility.UrlDecode(token));
                if (result)
                {
                    return Ok();
                }

                _logger.LogError("User has invalid token");
                return BadRequest("Invalid token");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to confirm user's email");
                return BadRequest();
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
                {
                    throw new Exception("Passwords were not the same.");
                }

                var result = await _userService.ResetPasswordAsync(resetPasswordDto);
                if (result)
                {
                    return Ok();
                }

                return BadRequest("Invalid password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to reset password");
                return BadRequest(ex.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("reset-password-with-token")]
        public async Task<ActionResult> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto resetPasswordTokenDto)
        {
            try
            {
                if (resetPasswordTokenDto.NewPassword != resetPasswordTokenDto.ConfirmNewPassword)
                {
                    throw new Exception("Passwords were not the same.");
                }
                
                var result = await _userService.ResetPasswordWithTokenAsync(resetPasswordTokenDto);
                if (result)
                {
                    return Ok();
                }

                return BadRequest("Invalid password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to reset password");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                await _userService.ForgotPassword(forgotPasswordDto);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("token")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenDto>> GetTokenAsync()
        {
            var username = "";
            try
            {
                var (u, password) = Request.GetAuthorization();
                username = u;
                return await _userService.LoginAsync(username, password);
            }
            catch (NoUserFoundException ex)
            {
                _logger.LogError(ex, $"No user found with username: {username}");
                return BadRequest(ex.Message);
            }
            catch (InvalidLoginCredentialException)
            {
                _logger.LogError($"Invalid credentials. Username: {username}");
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Invalid credentials. Username: {username}");
                return BadRequest("Something went wrong.");
            }
        }

        [HttpGet("logout")]
        public ActionResult RevokeToken()
        {
            var success = true;

            try
            {
                _userService.Logout(User.GetUserId());
            }
            catch
            {
                success = false;
            }

            return Ok(new Dictionary<string, bool>
            {
                {"Success", success}
            });
        }

        [HttpPatch("update/{id}")]
        public async Task<ActionResult<TokenDto>> PatchAsync(Guid id,
            [FromBody] JsonPatchDocument<UserDto> userPatchDto)
        {
            try
            {
                var updatedTokenDto = await _userService.PatchUserAsync(id, userPatchDto);

                if (await _distributedCache.ContainsKeyAsync(User.GetUserId()))
                {
                    var clientData = await _distributedCache.GetAsync<ClientData>(User.GetUserId());
                    await _graphHubContext.Clients
                        .Clients(clientData.Bridges.Select(x => x.GraphHubConnectionId).ToList())
                        .SendAsync("UpdatedUserAsync", updatedTokenDto);
                }

                return Ok(updatedTokenDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to patch user");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<UserDto>> UpdateAsync([FromBody] UserDto userDto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(userDto);
                if (result)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to update user");
                return BadRequest();
            }
        }

        [HttpDelete("delete/{userId}")]
        public async Task<ActionResult> DeleteAsync(Guid userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to delete user");
                return BadRequest();
            }
        }
    }
}