using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Nodester.WebApi.Extensions
{
    public static class HttpRequestExtension
    {

        public static (string username, string password) GetAuthorization(this HttpRequest request)
        {
            var authorization = request.Headers["Authorization"].ToString();
            var encoded = authorization.Split(" ")[1];
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var info = decoded.Split(":");
            return (info[0], info[1]);
        }
        
    }
}