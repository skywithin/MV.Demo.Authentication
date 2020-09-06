using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MV.Demo.Client
{
    public static class Constants
    {
        //TODO: This should be in user secrets or appsettings

        public const string AuthenticationScheme = "ClientCookie";
        public const string ChallengeScheme = "OurServer";
        public const string ClientId = "client_id"; //"some_client_id";
        public const string ClientSecret = "client_secret"; //"some_long_client_secret";
        public const string ClientAuthCallbackPath = "/oauth/callback";

        public const string AuthServerBaseUrl = "https://localhost:44375/";
        public const string AuthServerAuthorizationEndpoint = AuthServerBaseUrl + "oauth/authorize";
        public const string AuthServerTokenEndpoint = AuthServerBaseUrl + "oauth/token";

    }
}
