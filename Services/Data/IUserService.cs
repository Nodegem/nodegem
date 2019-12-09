using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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
        Task<UserDto> GetByLoginInfoAsync(UserLoginInfo info);
        Task LinkLoginInfo(Guid userId, UserLoginInfo info);
        Task<TokenDto> RegisterAsync(RegisterDto dto, UserLoginInfo info = null);
        Task<bool> ConfirmEmailAsync(Guid userId, string token);
        Task<TokenDto> LoginAsync(string username, string password);
        Task<UserDto> GetUserAsync(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid userId);
        TokenDto RefreshToken(string token);
        Task<bool> UpdateUserAsync(UserDto user);
        Task<TokenDto> PatchUserAsync(Guid userId, JsonPatchDocument<UserDto> patchDocument);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        void Logout(Guid userId);
        void LockUser(UserDto dto);
        Task DeleteUserAsync(Guid userId);
        Task<TokenDto> GetTokenAsync(ApplicationUser user);
        string GeneratePassword();
    }
}