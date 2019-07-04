using System;
using System.Collections.Generic;
using Constant = Nodester.Common.Data.Constant;

namespace Nodester.Services.Data
{
    public interface ITokenService
    {
        (string token, DateTime expires) GenerateJwtToken(string email, string username, Guid userId,
            IEnumerable<Constant> constants);
    }
}