using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nodegem.Common.Dto.ComponentDtos;
using Nodegem.Data.Dto;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;

namespace Nodegem.Services.Data
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);

        Task<TokenDto> RegisterAsync(RegisterDto dto, UserLoginInfo info = null);
        Task<bool> ConfirmEmailAsync(Guid userId, string token);
        Task<TokenDto> LoginAsync(string username, string password);
        Task<UserDto> GetUser(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid userId);
        TokenDto RefreshToken(string token);
        Task<bool> UpdateUserAsync(UserDto user);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        void Logout(Guid userId);
        void LockUser(UserDto dto);
        Task DeleteUserAsync(Guid userId);
        TokenDto GetToken(UserDto user);
        TokenDto GetToken(ApplicationUser user);
        string GeneratePassword();
    }
}