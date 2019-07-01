using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nodester.Data.Dto.ComponentDtos;
using Nodester.Data.Dto.UserDtos;

namespace Nodester.Services.Data
{
    public interface IUserService
    {
        Task<TokenUserDto> RegisterAsync(RegisterDto dto);
        Task<TokenUserDto> LoginAsync(string username, string password);
        Task<UserDto> GetUser(Guid userId);
        Task<IEnumerable<ConstantDto>> GetConstantsAsync(Guid userId);
        void UpdateUser();
        void ResetPassword();
        void Logout(Guid userId);
        void LockUser(UserDto dto);
        void DeleteUser();
    }
}