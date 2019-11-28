namespace Nodegem.ClientService.Data
{
    public interface INodegemApiService
    {
        INodegemLoginService LoginService { get; }
        INodegemGraphService GraphService { get; }
        INodegemUserService UserService { get; }
    }
}