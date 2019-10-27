using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nodester.Data.Dto;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.UserDtos;
using Nodester.Data.Models;

namespace Nodester.Services.Data
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<TokenDto> RegisterAsync(RegisterDto dto, UserLoginInfo info = null);
        Task<TokenDto> LoginAsync(string username, string password);
        Task<UserDto> GetUser(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid userId);
        TokenDto RefreshToken(string token);
        void UpdateUser();
        void ResetPassword();
        void Logout(Guid userId);
        void LockUser(UserDto dto);
        void DeleteUser();
        TokenDto GetToken(UserDto user);
        TokenDto GetToken(ApplicationUser user);
        string GeneratePassword();
    }
}