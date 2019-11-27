using System.Linq;
using Nodegem.Data.Contexts;
using Nodegem.Data.Models;
using Nodegem.Services.Data.Repositories;

namespace Nodegem.Services.Repositories
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