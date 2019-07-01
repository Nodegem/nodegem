using System.Threading.Tasks;
using Bridge.Data.Dtos;

namespace Bridge.Data
{
    public interface INodesterLoginService
    {

        Task<TokenDto> GetAccessTokenAsync(string username, string password);

    }
}