using System;
using System.Collections.Generic;
using System.Security.Claims;
using Nodegem.Data.Dto.UserDtos;
using Nodegem.Data.Models;
using Constant = Nodegem.Common.Data.Constant;

namespace Nodegem.Services.Data
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateJwtToken(UserDto user);

        (bool valid, ClaimsPrincipal user) IsValidToken(string token);
    }
}