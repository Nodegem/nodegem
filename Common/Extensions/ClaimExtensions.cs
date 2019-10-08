using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nodester.Common.Data;

namespace Nodester.Common.Extensions
{
    public static class ClaimExtensions
    {

        public const string ConstantClaimId = "constantData";
        
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.GetUserId();
        }
        
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            var claim = claims.GetClaim(JwtRegisteredClaimNames.NameId);
            if (claim == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return Guid.Parse(claim);
        }

        public static IEnumerable<Constant> GetConstants(this ClaimsPrincipal principal)
        {
            return principal.Claims.GetConstants();
        }
        
        public static IEnumerable<Constant> GetConstants(this IEnumerable<Claim> claims)
        {
            var constants = claims.GetClaim(ConstantClaimId);
            return JsonConvert.DeserializeObject<IEnumerable<Constant>>(constants);
        }
        
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.GetEmail();
        }
        
        public static string GetEmail(this IEnumerable<Claim> claims)
        {
            return claims.GetClaim(JwtRegisteredClaimNames.Sub);
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            return principal.Claims.GetUsername();
        }
        
        public static string GetUsername(this IEnumerable<Claim> claims)
        {
            return claims.GetClaim(JwtRegisteredClaimNames.UniqueName);
        }
        
        public static string GetClaim(this ClaimsPrincipal user, string claimName)
        {
            return user.Claims.GetClaim(claimName);
        }
        
        public static string GetClaim(this IEnumerable<Claim> claims, string claimName)
        {
            return claims.FirstOrDefault(x => x.Type == claimName)?.Value;
        }
    }
}