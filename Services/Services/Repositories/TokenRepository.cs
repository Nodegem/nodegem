using System;
using System.Linq;
using Nodester.Data.Contexts;
using Nodester.Data.Models;
using Nodester.Services.Data.Repositories;

namespace Nodester.Services.Repositories
{
    public class TokenRepository : Repository<AccessToken>, ITokenRepository
    {
        public TokenRepository(NodesterDBContext context) : base(context)
        {
        }

        public void AddAccessToken(AccessToken token)
        {
            var refreshToken = DbSet.SingleOrDefault(x => x.User.Id == token.UserId);
            if (refreshToken != null)
            {
//                DeleteRefreshToken(refreshToken);
            }

            Create(token);
        }

    }
}