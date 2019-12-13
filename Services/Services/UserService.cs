using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nodegem.Common.Data;
using Nodegem.Common.Extensions;
using Nodegem.Data.Dto;
using Nodegem.Data.Dto.EmailDtos;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;
using Nodegem.Data.Settings;
using Nodegem.Services.Data;
using Nodegem.Services.Exceptions;
using Nodegem.Services.Exceptions.Login;
using Nodegem.Services.Extensions;

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
            return foundUser?.DecryptUser(_dataProtector).Adapt<UserDto>();
        }

        public async Task<UserDto> GetByLoginInfoAsync(UserLoginInfo info)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return user?.DecryptUser(_dataProtector).Adapt<UserDto>();
        }

        public async Task LinkLoginInfo(Guid userId, UserLoginInfo info)
        {
            var login = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (login != null && login.Id == userId)
            {
                throw new Exception("Your account is already associated to this login provider");
            }

            if (login != null)
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

            var user = await FindUserAsync(dto.UserName, dto.Email);
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
            return await GetTokenAsync(user.Adapt<UserDto>());
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
            var user = await FindUserAsync(username);

            if (user == null)
            {
                throw new NoUserFoundException();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) throw new InvalidLoginCredentialException();

            await UpdateLastLoggedIn(user);
            return await GetTokenAsync(user.Adapt<UserDto>());
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            return (await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId)).Adapt<UserDto>();
        }

        public async Task<IEnumerable<Constant>> GetConstantsAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user?.Constants == null
                ? new List<Constant>()
                : user.Constants.Select(c => c.Adapt<Constant>());
        }

        public TokenDto RefreshToken(string token)
        {
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

            user = user.EncryptUser(_dataProtector);
            await _userManager.UpdateAsync(user);
            
            return await GetTokenAsync(user.Adapt<UserDto>());
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
            var user = await _userManager.FindByIdAsync(_context.User.GetUserId().ToString());
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
                        new ResetPasswordEmailDto()
                        {
                            Email = user.Email,
                            Username = user.UserName
                        });
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<bool> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto resetPasswordWithTokenDto)
        {
            var user = await _userManager.FindByIdAsync(resetPasswordWithTokenDto.UserId.ToString());
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user,
                    HttpUtility.UrlDecode(resetPasswordWithTokenDto.ResetToken),
                    resetPasswordWithTokenDto.NewPassword);
                if (result.Succeeded)
                {
                    await _emailService.SendEmailAsync(
                        "Nodegem - Reset Password",
                        user.Email,
                        "ResetPassword",
                        new ResetPasswordEmailDto
                        {
                            Email = user.Email,
                            Username = user.UserName
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

        private async Task<ApplicationUser> FindUserAsync(string userNameOrEmail)
        {
            return await FindUserAsync(userNameOrEmail, userNameOrEmail);
        }

        private async Task<ApplicationUser> FindUserAsync(string userName, string email)
        {
            userName = userName.ToLower();
            email = email.ToLower();
            return await _userManager.Users.SingleOrDefaultAsync(x =>
                x.NormalizedUserName == userName.ToUpper() || x.NormalizedEmail == email.ToUpper());
        }

        private async Task UpdateLastLoggedIn(ApplicationUser user)
        {
            user.LastLoggedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        public async Task<TokenDto> GetTokenAsync(UserDto user)
        {
            // Wish this method took a fuckin ID instead of the whole object
            var providers = await _userManager.GetLoginsAsync(user.Adapt<ApplicationUser>());
            
            user.Providers = providers.Select(x => x.ProviderDisplayName).ToList();
            user = user.DecryptUser(_dataProtector);
            var (accessToken, expires) =
                _tokenService.GenerateJwtToken(user);
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
    }
}