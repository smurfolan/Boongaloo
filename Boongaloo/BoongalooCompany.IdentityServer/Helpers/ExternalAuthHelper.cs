using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;
using Owin.Security.Providers.LinkedIn;

namespace BoongalooCompany.IdentityServer.Helpers
{
    public static class ExternalAuthHelper
    {
        public static void ConfigureExternalAuthProviders(IAppBuilder app, string signInAsType)
        {
            GoogleAuthentication(app, signInAsType);
        }

        private static void GoogleAuthentication(IAppBuilder app, string signInAsType)
        {
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                Caption = "Google",
                SignInAsAuthenticationType = signInAsType,
                ClientId = "97756425623-pf0l0olj4d0bjiuaslg5cmd330g4bit9.apps.googleusercontent.com",
                ClientSecret = "y68P2OYWJtsb3yWl9rR_DsyS",
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = (context) =>
                    {

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}