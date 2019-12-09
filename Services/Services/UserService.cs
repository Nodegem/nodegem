using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nodegem.Common.Data;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Common.Extensions;
using Nodegem.Data.Dto;
using Nodegem.Data.Dto.EmailDtos;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;
using Nodegem.Data.Settings;
using Nodegem.Services.Data;
using Nodegem.Services.Exceptions;
using Nodegem.Services.Exceptions.Login;

namespace Nodegem.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HttpContext _context;
        private readonly ITokenService _tokenService;
        private readonly ISendEmails _emailService;
        private readonly AppSettings _appSettings;
        private readonly IDataProtector _dataProtector;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ISendEmails emailService,
            IHttpContextAccessor contextAccessor,
            IOptions<AppSettings> appSettings,
            IDataProtectionProvider protectionProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = contextAccessor.HttpContext;
            _appSettings = appSettings.Value;
            _dataProtector = protectionProvider.CreateProtector(_appSettings.SecretKey);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var foundUser = await _userManager.FindByEmailAsync(email);
            return foundUser == null ? null : UnprotectUser(foundUser);
        }

        public async Task<UserDto> GetByLoginInfoAsync(UserLoginInfo info)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return user.Adapt<UserDto>();
        }

        public async Task LinkLoginInfo(Guid userId, UserLoginInfo info)
        {
            var login = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (login != null && login.Id == userId)
            {
                throw new Exception("Your account is already associated to this login provider");
            }
            
            if(login != null)
            {
                throw new Exception("An account is already associated to this login provider");
            }
            
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.AddLoginAsync(user, info);
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

            var user = FindUser(dto.UserName, dto.Email);
            if (info != null)
            {
                await _userManager.AddLoginAsync(user, info);
            }
            else
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailService.SendEmailAsync(
                    "Welcome to Nodegem!",
                    dto.Email, "Registration",
                    new RegisterUserDto
                    {
                        Email = user.Email,
                        Username = user.UserName,
                        EmailConfirmationToken = token,
                        UserId = user.Id.ToString(),
                        Host = _appSettings.Host
                    });
            }

            await _signInManager.SignInAsync(user, false);
            await UpdateLastLoggedIn(user);
            return await GetTokenAsync(user);
        }

        public async Task<bool> ConfirmEmailAsync(Guid userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token.Replace(' ', '+'));
                return result.Succeeded;
            }

            return false;
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
            return await GetTokenAsync(user);
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
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
//            var (isValid, user) = _tokenService.IsValidToken(token);
//            if (!isValid) throw new UnauthorizedAccessException();
//
//            var (newToken, expires) = _tokenService.GenerateJwtToken(user.GetEmail(), user.GetUsername(),
//                user.GetAvatarUrl(),
//                user.GetUserId(),
//                user.GetConstants());
//            return new TokenDto
//            {
//                AccessToken = newToken,
//                ExpiresUtc = expires,
//                IssuedUtc = DateTime.UtcNow
//            };

            return new TokenDto();
        }

        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null) return false;
            var result = await _userManager.UpdateAsync(user.Adapt<ApplicationUser>());
            return result.Succeeded;
        }

        public async Task<TokenDto> PatchUserAsync(Guid userId, JsonPatchDocument<UserDto> patchDocument)
        {
            var appUserDocument = patchDocument.Adapt<JsonPatchDocument<ApplicationUser>>();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            appUserDocument.ApplyTo(user);
            await _userManager.UpdateAsync(user);
            return await GetTokenAsync(user);
        }

        public void LockUser(UserDto dto)
        {
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _emailService.SendEmailAsync(
                    "Nodegem - Forgot Password",
                    user.Email,
                    "ForgotPassword",
                    new ForgotPasswordEmailDto
                    {
                        Email = user.Email,
                        Username = user.UserName,
                        Host = _appSettings.Host,
                        UserId = user.Id.ToString(),
                        ResetPasswordToken = resetToken
                    });
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(_context.User.GetEmail());
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, resetPasswordDto.CurrentPassword,
                    resetPasswordDto.NewPassword);
                if (result.Succeeded)
                {
                    await _emailService.SendEmailAsync(
                        "Nodegem - Reset Password",
                        user.Email,
                        "ResetPassword",
                        new ForgotPasswordEmailDto
                        {
                            Email = user.Email,
                            Username = user.UserName,
                            Host = _appSettings.Host
                        });
                    return true;
                }

                return false;
            }

            return false;
        }

        public void Logout(Guid userId)
        {
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
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

        public async Task<TokenDto> GetTokenAsync(ApplicationUser user)
        {
            var userDto = UnprotectUser(user);
            var providers = await _userManager.GetLoginsAsync(user);
            userDto.Providers = providers.Select(x => x.ProviderDisplayName).ToList();
            var (accessToken, expires) =
                _tokenService.GenerateJwtToken(userDto);
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

            var nonAlphanumeric = options.RequireNonAlphanumeric;
            var digit = options.RequireDigit;
            var lowercase = options.RequireLowercase;
            var uppercase = options.RequireUppercase;

            var password = new StringBuilder();
            var random = new Random();

            var length = random.Next(options.RequiredLength + 8, options.RequiredLength + 24);

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

        private ApplicationUser ProtectUser(UserDto user)
        {
            var userEntity = user.Adapt<ApplicationUser>();
            userEntity.Constants = userEntity.Constants.Select(x =>
            {
                var constant = x.Adapt<Nodegem.Data.Models.Json_Models.Graph_Constants.Constant>();
                constant.Value = constant.IsSecret
                    ? _dataProtector.Protect(constant.Value.ToString())
                    : constant.Value;
                return constant;
            });
            return userEntity;
        }

        private UserDto UnprotectUser(ApplicationUser user)
        {
            var userDto = user.Adapt<UserDto>();
            userDto.Constants = userDto.Constants.Select(x =>
            {
                var constant = x.Adapt<ConstantDto>();
                constant.Value = constant.IsSecret
                    ? _dataProtector.Unprotect(constant.Value.ToString())
                    : constant.Value;
                return constant;
            });
            return userDto;
        }
    }
}