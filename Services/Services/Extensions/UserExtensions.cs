using Microsoft.AspNetCore.DataProtection;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;

namespace Nodegem.Services.Extensions
{
    public static class UserExtensions
    {
        public static UserDto EncryptedUser(this UserDto user, IDataProtector protector)
        {
            user.Constants = user.Constants.EncryptConstants(protector);
            return user;
        }

        public static UserDto DecryptUser(this UserDto user, IDataProtector protector)
        {
            user.Constants = user.Constants.DecryptConstants(protector);
            return user;
        }
        
        public static ApplicationUser EncryptedUser(this ApplicationUser user, IDataProtector protector)
        {
            user.Constants = user.Constants.EncryptConstants(protector);
            return user;
        }

        public static ApplicationUser DecryptUser(this ApplicationUser user, IDataProtector protector)
        {
            user.Constants = user.Constants.DecryptConstants(protector);
            return user;
        }
    }
}