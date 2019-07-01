using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nodester.Data.Dto;
using Nodester.Data.Models;
using Nodester.Data.Settings;
using Nodester.Services.Data;
using Nodester.Services.Data.Mappers;
using Nodester.Services.Data.Repositories;
using Nodester.Services.Extensions;

namespace Nodester.Services
{
    public class TokenService : ITokenService
    {
        private ITokenRepository _tokenRepository;
        private IMapper<AccessToken, TokenDto> _tokenMapper;
        private TokenSettings _tokenSettings;

        public TokenService(ITokenRepository tokenRepository,
            IMapper<AccessToken, TokenDto> tokenMapper,
            IOptions<TokenSettings> tokenSettings)
        {
            _tokenRepository = tokenRepository;
            _tokenMapper = tokenMapper;
            _tokenSettings = tokenSettings.Value;
        }

        public (string token, DateTime expires) GenerateJwtToken(string email, string username, Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username)
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