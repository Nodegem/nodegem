using System;

namespace Nodegem.Services.Exceptions.Login
{
    public class InvalidLoginCredentialException : Exception
    {
        public InvalidLoginCredentialException() : base("Invalid username or password")
        {
        }
    }
}