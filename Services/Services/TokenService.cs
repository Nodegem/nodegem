using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Nodegem.Common.Extensions;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Settings;
using Nodegem.Services.Data;

namespace Nodegem.Services
{
    public class TokenService : ITokenService
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
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

        public (string token, DateTime expires) GenerateJwtToken(UserDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(ClaimExtensions.AvatarClaimId, user.AvatarUrl ?? ""),
                new Claim(ClaimExtensions.ProvidersClaimId, JsonConvert.SerializeObject(user.Providers,
                    SerializerSettings)),
                new Claim(ClaimExtensions.ConstantClaimId,
                    JsonConvert.SerializeObject(user.Constants.Select(x => x.Adapt<Common.Data.Constant>()),
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