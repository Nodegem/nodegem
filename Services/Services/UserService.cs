using Microsoft.AspNetCore.Identity;
using Nodester.Data.Dto.UserDtos;
using Nodester.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Mapster;
using Nodester.Services.Data;
using Nodester.Services.Exceptions;
using Nodester.Services.Exceptions.Login;
using Microsoft.EntityFrameworkCore;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Data.Dto;
using Nodester.Data.Dto.ComponentDtos;

namespace Nodester.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ISendEmail _emailService;
        private IUserService _userServiceImplementation;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ISendEmail emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            return (await _userManager.FindByEmailAsync(email)).Adapt<UserDto>();
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto dto, UserLoginInfo info = null)
        {
            var registerUser = dto.Adapt<ApplicationUser>();

            var time = DateTime.UtcNow;
            registerUser.CreatedOn = time;
            registerUser.LastUpdated = time;
            registerUser.LastLoggedIn = time;

            var result = await _userManager.CreateAsync(registerUser, dto.Password);
            if (!result.Succeeded) throw new RegistrationException(result);

//            await _emailService.SendEmailAsync(registerUser.Email, "Hello");

            var user = FindUser(dto.UserName, dto.Email);
            if (info != null)
            {
                await _userManager.AddLoginAsync(user, info);
            }

            await _signInManager.SignInAsync(user, false);
            await UpdateLastLoggedIn(user);
            return GetToken(user);
        }

        public async Task<TokenDto> LoginAsync(string username, string password)
        {
            var user = FindUser(username);

            if (user == null)
            {
                throw new NoUserFoundException();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) throw new InvalidLoginCredentialException();

            await UpdateLastLoggedIn(user);
            return GetToken(user);
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

        public TokenDto RefreshToken(string token)
        {
            var (isValid, user) = _tokenService.IsValidToken(token);
            if (!isValid) throw new UnauthorizedAccessException();

            var (newToken, expires) = _tokenService.GenerateJwtToken(user.GetEmail(), user.GetUsername(),
                user.GetAvatarUrl(),
                user.GetUserId(),
                user.GetConstants());
            return new TokenDto
            {
                AccessToken = newToken,
                ExpiresUtc = expires,
                IssuedUtc = DateTime.UtcNow
            };
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
            userName = userName.ToLower();
            email = email.ToLower();
            return _userManager.Users.SingleOrDefault(x =>
                x.NormalizedUserName == userName.ToUpper() || x.NormalizedEmail == email.ToUpper());
        }

        private async Task UpdateLastLoggedIn(ApplicationUser user)
        {
            user.LastLoggedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        public TokenDto GetToken(UserDto user)
        {
            return GetToken(user.Adapt<ApplicationUser>());
        }

        public TokenDto GetToken(ApplicationUser user)
        {
            var (accessToken, expires) =
                _tokenService.GenerateJwtToken(user.Email, user.UserName, user.AvatarUrl, user.Id,
                    user.Constants?.Select(x => x.Adapt<Constant>()));
            return new TokenDto
            {
                AccessToken = accessToken,
                IssuedUtc = DateTime.Now,
                ExpiresUtc = expires
            };
        }

        public string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            var length = options.RequiredLength;

            var nonAlphanumeric = options.RequireNonAlphanumeric;
            var digit = options.RequireDigit;
            var lowercase = options.RequireLowercase;
            var uppercase = options.RequireUppercase;

            var password = new StringBuilder();
            var random = new Random();

            while (password.Length < length)
            {
                var c = (char) random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char) random.Next(33, 48));
            if (digit)
                password.Append((char) random.Next(48, 58));
            if (lowercase)
                password.Append((char) random.Next(97, 123));
            if (uppercase)
                password.Append((char) random.Next(65, 91));

            return password.ToString();
        }
    }
}