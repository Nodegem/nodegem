using System.Threading.Tasks;
using Nodegem.Data.Dto;

namespace Nodegem.ClientService.Data
{
    public interface INodesterLoginService
    {

        Task<TokenDto> GetAccessTokenAsync(string username, string password);

    }
}