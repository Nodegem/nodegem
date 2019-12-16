using System;

namespace Nodegem.Services.Exceptions.Login
{
    public class NoUserFoundException : Exception
    {
        public NoUserFoundException() : base("No user found with username/email")
        {
        }
    }
}