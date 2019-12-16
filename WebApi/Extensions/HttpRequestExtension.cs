using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Nodegem.WebApi.Extensions
{
    public static class HttpRequestExtension
    {

        public static (string username, string password) GetAuthorization(this HttpRequest request)
        {
            try
            {
                var authorization = request.Headers["Authorization"].ToString();
                var encoded = authorization.Split(" ")[1];
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                var info = decoded.Split(":");
                return (info[0], info[1]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("No credentials were provided");
            }

        }
        
    }
}