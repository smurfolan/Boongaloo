
namespace Boongaloo
{
    public class Constants
    {
        public const string BoongalooIOSCallback = "com.boongaloo.ios";

        public const string BoongalooAPI = "https://boongalooapi.azurewebsites.net/";
        public const string BoongalooMVC = "https://localhost:44318/";
        public const string BoongalooMVCSTSCallback = "https://localhost:44318/stscallback";
        public const string BoongalooAngular = "https://localhost:44316/";

        public const string BoongalooClientSecret = "myrandomclientsecret";

        public const string BoongalooIssuerUri = "https://boongaloocompanysts/identity";
        public const string BoongalooSTSOrigin = "https://boongalooidsrv.azurewebsites.net";
        public const string BoongalooSTS = BoongalooSTSOrigin + "/identity";
        public const string BoongalooSTSTokenEndpoint = BoongalooSTS + "/connect/token";
        public const string BoongalooSTSAuthorizationEndpoint = BoongalooSTS + "/connect/authorize";
        public const string BoongalooSTSUserInfoEndpoint = BoongalooSTS + "/connect/userinfo";
        public const string BoongalooSTSEndSessionEndpoint = BoongalooSTS + "/connect/endsession";
        public const string BoongalooSTSRevokeTokenEndpoint = BoongalooSTS + "/connect/revocation";


    }

}
