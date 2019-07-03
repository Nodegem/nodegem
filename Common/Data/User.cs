using System;
using System.Collections.Generic;

namespace Nodester.Common.Data
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public Dictionary<Guid, Constant> Constants { get; set; }

    }
}