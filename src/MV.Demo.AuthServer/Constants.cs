using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MV.Demo.AuthServer
{
    public static class Constants
    {
        //TODO: This should be in user secrets or appsettings

        public const string AuthenticationScheme = "OAuth"; //Name of the authentication scheme

        public const string Issuer = Audience; // Server is issuing a token to iteself
        public const string Audience = "https://localhost:44375/"; //Server self
        public const string Secret = "not_too_short_secret_otherwise_might_fail";
    }
}
