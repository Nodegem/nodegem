using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Data.Dto.GraphDtos;

namespace Nodester.Bridge
{
    public class AppState
    {

        private static AppState _instance;
        public static AppState Instance => _instance ?? (_instance = new AppState());

        public bool IsLoggedIn => Token != null;

        public JwtSecurityToken Token { get; set; }

        public Guid UserId => Token.Claims.GetUserId();
        public string Email => Token.Claims.GetEmail();
        public string Environment { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public IEnumerable<GraphDto> Graphs { get; set; }

        public User User => new User
        {
            Id = UserId.ToString(),
            Email = Email,
            Username = Username,
            Constants = Token.Claims.GetConstants().ToDictionary(k => k.Key, v => v)
        };
    }
}