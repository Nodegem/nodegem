using Nodegem.Data.Models;

namespace Nodegem.Services.Data.Repositories
{
    public interface ITokenRepository
    {
        void AddAccessToken(AccessToken token);
    }
}