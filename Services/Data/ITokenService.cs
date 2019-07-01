using System;
using Nodester.Data.Dto;
using Nodester.Data.Models;

namespace Nodester.Services.Data
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateJwtToken(string email, string username, Guid userId);
    }
}