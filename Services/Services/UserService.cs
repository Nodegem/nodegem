using Microsoft.AspNetCore.Identity;
using Nodester.Data.Dto.UserDtos;
using Nodester.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Mapster;
using Nodester.Services.Data;
using Nodester.Services.Exceptions;
using Nodester.Services.Exceptions.Login;
using Microsoft.EntityFrameworkCore;
using Nodester.Data.Dto;
using Nodester.Data.Dto.ComponentDtos;

namespace Nodester.Services
{
    public class UserService : IUserService
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private ITokenService _tokenService;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<TokenUserDto> RegisterAsync(RegisterDto dto)
        {
            var registerUser = dto.Adapt<ApplicationUser>();

            var time = DateTime.UtcNow;
            registerUser.CreatedOn = time;
            registerUser.LastUpdated = time;
            registerUser.LastLoggedIn = time;

            var result = await _userManager.CreateAsync(registerUser, dto.Password);
            if (result.Succeeded)
            {
                var user = FindUser(dto.UserName, dto.Email);
                await _signInManager.SignInAsync(user, false);
                await UpdateLastLoggedIn(user);
                return GetUserWithToken(user);
            }

            throw new RegistrationException(result);
        }

        public async Task<TokenUserDto> LoginAsync(string username, string password)
        {
            var user = FindUser(username);

            if (user == null)
            {
                throw new NoUserFoundException();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                await UpdateLastLoggedIn(user);
                return GetUserWithToken(user);
            }

            throw new InvalidLoginCredentialException();
        }

        public async Task<UserDto> GetUser(Guid userId)
        {
            return (await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId)).Adapt<UserDto>();
        }

        public async Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user?.Constants == null
                ? new List<ConstantDto>()
                : user.Constants.Select(c => c.Adapt<ConstantDto>());
        }

        public void UpdateUser()
        {
            throw new NotImplementedException();
        }

        public void LockUser(UserDto dto)
        {
        }

        public void Logout(Guid userId)
        {
        }

        public void ResetPassword()
        {
            throw new NotImplementedException();
        }

        public void DeleteUser()
        {
            throw new NotImplementedException();
        }

        private ApplicationUser FindUser(string userNameOrEmail)
        {
            return FindUser(userNameOrEmail, userNameOrEmail);
        }

        private ApplicationUser FindUser(string userName, string email)
        {
            return _userManager.Users.SingleOrDefault(x => x.UserName == userName || x.Email == email);
        }

        private async Task UpdateLastLoggedIn(ApplicationUser user)
        {
            user.LastLoggedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        private TokenUserDto GetUserWithToken(ApplicationUser user)
        {
            var (accessToken, expires) = _tokenService.GenerateJwtToken(user.Email, user.UserName, user.Id);
            var userDto = user.Adapt<UserDto>();

            return new TokenUserDto
            {
                Token = new TokenDto
                {
                    AccessToken = accessToken,
                    IssuedUtc = DateTime.Now,
                    ExpiresUtc = expires
                },
                User = userDto
            };
        }
    }
}