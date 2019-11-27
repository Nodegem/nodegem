using System;
using System.Collections.Generic;
using System.Security.Claims;
using Constant = Nodegem.Common.Data.Constant;

namespace Nodegem.Services.Data
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateJwtToken(string email, string username, string avatarUrl, Guid userId,
            IEnumerable<Constant> constants);

        (bool valid, ClaimsPrincipal user) IsValidToken(string token);
    }
}