using System.Threading.Tasks;
using Nodester.Services.Data;

namespace Nodester.Services
{
    public class EmailService : ISendEmail
    {
        public Task<bool> SendEmailAsync(string to, string content)
        {
            throw new System.NotImplementedException();
        }
    }
}