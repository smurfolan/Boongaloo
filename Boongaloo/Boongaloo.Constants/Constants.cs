
namespace Boongaloo
{
    public class Constants
    {
        public const string BoongalooIOSCallback = "app:/com.boongaloo.ios";
        public const string BoongalooIOSPostLogoutCallback = "app:/com.boongaloo.ios.logout";

        public const string BoongalooMvcAuthCodePostLogoutCallback =
            "https://localhost:44346/Home/StsCallBackForAuthCodeClient";

        public const string BoongalooAPI = "http://likkleapi.azurewebsites.net/";
        public const string BoongalooMVC = "https://localhost:44318/";
        public const string BoongalooMVCSTSCallback = "https://localhost:44318/stscallback";
        public const string BoongalooAngular = "https://localhost:44316/";

        public const string BoongalooClientSecret = "myrandomclientsecret";

        public const string BoongalooIssuerUri = "https://boongaloocompanysts/identity";
        public const string BoongalooSTSOrigin = "https://boongalooidsrv.azurewebsites.net"; //"https://localhost:44317";
        public const string BoongalooSTS = BoongalooSTSOrigin + "/identity";
        public const string BoongalooSTSTokenEndpoint = BoongalooSTS + "/connect/token";
        public const string BoongalooSTSAuthorizationEndpoint = BoongalooSTS + "/connect/authorize";
        public const string BoongalooSTSUserInfoEndpoint = BoongalooSTS + "/connect/userinfo";
        public const string BoongalooSTSEndSessionEndpoint = BoongalooSTS + "/connect/endsession";
        public const string BoongalooSTSRevokeTokenEndpoint = BoongalooSTS + "/connect/revocation";


    }

}
