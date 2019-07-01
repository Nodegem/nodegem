using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Nodester.Common.Extensions
{
    public static class ClaimExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.GetClaim(JwtRegisteredClaimNames.NameId);
            if (claim == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return Guid.Parse(claim);
        }
        
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.GetClaim(JwtRegisteredClaimNames.Sub);
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            return principal.GetClaim(JwtRegisteredClaimNames.UniqueName);
        }
        
        public static string GetClaim(this ClaimsPrincipal user, string claimName)
        {
            return user.Claims.FirstOrDefault(x => x.Type == claimName)?.Value;
        }
    }
}