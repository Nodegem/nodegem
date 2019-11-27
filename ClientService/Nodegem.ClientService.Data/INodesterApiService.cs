namespace Nodegem.ClientService.Data
{
    public interface INodesterApiService
    {
        INodesterLoginService LoginService { get; }
        INodesterGraphService GraphService { get; }
        INodesterUserService UserService { get; }
    }
}