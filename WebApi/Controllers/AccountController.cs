using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nodester.Common.Extensions;
using Nodester.Data.Dto;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.UserDtos;
using Nodester.Services.Data;
using Nodester.Services.Exceptions;
using Nodester.Services.Exceptions.Login;
using Nodester.WebApi.Extensions;

namespace Nodester.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(IEnumerable<IdentityError>))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenUserDto>> Register([FromBody] RegisterDto dto)
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
                return BadRequest(message);
            }
        }

        [AllowAnonymous]
        [HttpGet("login")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenUserDto>> Login()
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
        }

        [HttpGet("constants")]
        public async Task<ActionResult<IEnumerable<ConstantDto>>> GetUserConstantsAsync()
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
        [HttpGet("token")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenDto>> GetToken()
        {
            var username = "";
            try
            {
                var (u, password) = Request.GetAuthorization();
                username = u;
                var user = await _userService.LoginAsync(username, password);
                return user.Token;
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

        [HttpPut("update")]
        public ActionResult<UserDto> Update([FromBody] UserDto userDto)
        {
            return Ok();
        }
    }
}