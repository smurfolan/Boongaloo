
namespace Boongaloo
{
    public class Constants
    {

        public const string BoongalooAPI = "https://localhost:44315/";
        public const string BoongalooMVC = "https://localhost:44318/";
        public const string BoongalooMVCSTSCallback = "https://localhost:44318/stscallback";
        public const string BoongalooAngular = "https://localhost:44316/";

        public const string BoongalooClientSecret = "myrandomclientsecret";

        public const string BoongalooIssuerUri = "https://tripcompanysts/identity";
        public const string BoongalooSTSOrigin = "https://localhost:44317";
        public const string BoongalooSTS = BoongalooSTSOrigin + "/identity";
        public const string BoongalooSTSTokenEndpoint = BoongalooSTS + "/connect/token";
        public const string BoongalooSTSAuthorizationEndpoint = BoongalooSTS + "/connect/authorize";
        public const string BoongalooSTSUserInfoEndpoint = BoongalooSTS + "/connect/userinfo";
        public const string BoongalooSTSEndSessionEndpoint = BoongalooSTS + "/connect/endsession";
        public const string BoongalooSTSRevokeTokenEndpoint = BoongalooSTS + "/connect/revocation";


    }

}
