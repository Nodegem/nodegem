using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Nodester.Services.Exceptions
{
    public class RegistrationException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; set; }

        public RegistrationException(IdentityResult result) : base("Unable to register new user")
        {
            Errors = result.Errors;
        }
    }
}