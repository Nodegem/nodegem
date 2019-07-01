using System;
using Nodester.Data.Models;

namespace Nodester.Services.Data.Repositories
{
    public interface ITokenRepository
    {
        void AddAccessToken(AccessToken token);
    }
}