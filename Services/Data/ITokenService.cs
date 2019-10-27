using System;
using System.Collections.Generic;
using System.Security.Claims;
using Constant = Nodester.Common.Data.Constant;

namespace Nodester.Services.Data
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateJwtToken(string email, string username, string avatarUrl, Guid userId,
            IEnumerable<Constant> constants);

        (bool valid, ClaimsPrincipal user) IsValidToken(string token);
    }
}