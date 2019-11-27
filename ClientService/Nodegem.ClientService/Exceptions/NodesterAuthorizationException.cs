using System;

namespace Nodegem.ClientService.Exceptions
{
    public class NodesterAuthorizationException : Exception
    {
        public NodesterAuthorizationException() : base("Invalid token or not logged in to service")
        {
        }
    }
}