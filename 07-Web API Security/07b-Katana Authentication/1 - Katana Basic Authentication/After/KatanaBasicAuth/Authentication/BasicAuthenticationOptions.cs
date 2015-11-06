using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace KatanaBasicAuth.Authentication
{
    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationOptions(string realm, 
            Func<string, string, Task<bool>> validator) 
            : base(authenticationType: "Basic")
        {
            Realm = realm;
            Validator = validator;
        }

        public string Realm { get; private set; }
        public Func<string, string, Task<bool>> Validator { get; private set; }
    }
}