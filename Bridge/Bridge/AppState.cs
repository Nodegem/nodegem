using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Nodester.Common.Data;
using Nodester.Common.Extensions;
using Nodester.Data;
using Nodester.Data.Dto.GraphDtos;
using Nodester.Data.Models;

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
        
        public BridgeInfo Info { get; set; }

        public IEnumerable<GraphDto> Graphs => GraphLookUp.Values;
        public IDictionary<Guid, GraphDto> GraphLookUp { get; set; }

        public IEnumerable<GraphDto> RecurringGraphs => Graphs.Where(x => x.Type == ExecutionType.Recurring);
        public IEnumerable<GraphDto> ListenerGraphs => Graphs.Where(x => x.Type == ExecutionType.Listener);

        public User User => new User
        {
            Id = UserId.ToString(),
            Email = Email,
            Username = Username,
            Constants = Token.Claims.GetConstants().ToDictionary(k => k.Key, v => v)
        };
    }
}