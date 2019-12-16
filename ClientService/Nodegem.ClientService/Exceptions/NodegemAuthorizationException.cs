using System;

namespace Nodegem.ClientService.Exceptions
{
    public class NodegemAuthorizationException : Exception
    {
        public NodegemAuthorizationException() : base("Invalid token or not logged in to service")
        {
        }
    }
}