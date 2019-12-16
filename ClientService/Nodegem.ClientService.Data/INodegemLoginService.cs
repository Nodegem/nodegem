using System.Threading.Tasks;
using Nodegem.Common.Data;
using Nodegem.Data.Dto;

namespace Nodegem.ClientService.Data
{
    public interface INodegemLoginService
    {

        Task<TokenDto> GetAccessTokenAsync(string username, string password);

    }
}