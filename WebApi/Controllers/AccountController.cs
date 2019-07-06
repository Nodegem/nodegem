using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
                return BadRequest(ex.Errors);
            }
        }

        [AllowAnonymous]
        [HttpGet("login")]
        [ProducesResponseType(200, Type = typeof(TokenUserDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(401)]
        public async Task<ActionResult<TokenUserDto>> Login()
        {
            try
            {
                var (username, password) = Request.GetAuthorization();
                return await _userService.LoginAsync(username, password);
            }
            catch (NoUserFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidLoginCredentialException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("constants")]
        public async Task<ActionResult<IEnumerable<ConstantDto>>> GetUserConstantsAsync()
        {
            try
            {
                return Ok(await _userService.GetConstantsAsync(User.GetUserId()));
            }
            catch (Exception ex)
            {
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
            try
            {
                var (username, password) = Request.GetAuthorization();
                var user = await _userService.LoginAsync(username, password);
                return user.Token;
            }
            catch (NoUserFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidLoginCredentialException)
            {
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