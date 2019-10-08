using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
                return (true, user);
            }
            catch
            {
                return (false, null);
            }
        }

        public (string token, DateTime expires) GenerateJwtToken(string email, string username, Guid userId,
            IEnumerable<Common.Data.Constant> constants)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(ClaimExtensions.ConstantClaimId,
                    JsonConvert.SerializeObject(constants ?? new List<Common.Data.Constant>(), SerializerSettings))
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