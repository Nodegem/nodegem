using System;
using System.ComponentModel.DataAnnotations;

namespace Nodester.Data.Dto.UserDtos
{
    public class RegisterDto
    {
        [Required] public string UserName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string AvatarUrl { get; set; }

        [Required] public string Password { get; set; }
    }
}