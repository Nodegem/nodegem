using System.Threading.Tasks;
using Nodester.Data.Dto;

namespace Bridge.Data
{
    public interface INodesterLoginService
    {

        Task<TokenDto> GetAccessTokenAsync(string username, string password);

    }
}