using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nodester.Common.Extensions;
using Nodester.Data.Settings;
using Nodester.Services.Data;

namespace Nodester.Services
{
    public class TokenService : ITokenService
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly TokenSettings _tokenSettings;

        public TokenService(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public (bool valid, ClaimsPrincipal user) IsValidToken(string oldToken)
        {
            var validationParameters =
                new TokenValidationParameters
                {
                    ValidIssuer = _tokenSettings.Issuer,
                    ValidAudience = _tokenSettings.Audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = false,
                    ValidateIssuer = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                    ClockSkew = TimeSpan.Zero,
                };

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var user = handler.ValidateToken(oldToken, validationParameters, out _);

                var expires = long.Parse(user.GetClaim(JwtRegisteredClaimNames.Exp));
                var fileTimeUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(expires);
                var bufferPeriod = TimeSpan.FromSeconds(_tokenSettings.ExpirationBuffer);
                return DateTime.UtcNow - fileTimeUTC < bufferPeriod
                    ? (true, user)
                    : (false, null);
            }
            catch
            {
                return (false, null);
            }
        }

        public (string token, DateTime expires) GenerateJwtToken(string email, string username, string avatarUrl,
            Guid userId,
            IEnumerable<Common.Data.Constant> constants)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimExtensions.AvatarClaimId, avatarUrl ?? ""),
                new Claim(ClaimExtensions.ConstantClaimId,
                    JsonConvert.SerializeObject(constants ?? Enumerable.Empty<Common.Data.Constant>(),
                        SerializerSettings))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddSeconds(_tokenSettings.Expiration);

            var token = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}