using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nodegem.Common.Dto.ComponentDtos;

namespace Nodegem.Data.Dto.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool IsActive { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
        public IEnumerable<ConstantDto> Constants { get; set; }

        public List<string> Providers = new List<string>();
        
        [JsonIgnore]
        public string SecurityStamp { get; set; }
        
        [JsonIgnore]
        public DateTime LastLoggedIn { get; set; }

    }
}