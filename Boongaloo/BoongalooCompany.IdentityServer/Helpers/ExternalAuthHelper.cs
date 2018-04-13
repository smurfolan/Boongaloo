using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;

namespace BoongalooCompany.IdentityServer.Helpers
{
    public static class ExternalAuthHelper
    {
        public static void ConfigureExternalAuthProviders(IAppBuilder app, string signInAsType)
        {
            GoogleAuthentication(app, signInAsType);
            FacebookAuthentication(app, signInAsType);
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

        private static void FacebookAuthentication(IAppBuilder app, string signInAsType)
        {
            var fbOptions = new FacebookAuthenticationOptions()
            {
                AuthenticationType = "Facebook",
                Caption = "Facebook",
                SignInAsAuthenticationType = signInAsType,
                AppId = "1743032272668435",
                AppSecret = "3addd5c09d99166f0cb2469164368016",
                Provider = new FacebookAuthenticationProviderWrapper()
                {
                    OnAuthenticated = (context) =>
                    {
                        return Task.FromResult(0);
                    }
                },
                UserInformationEndpoint = "https://graph.facebook.com/v2.5/me?fields=id,name,first_name,last_name,email"
            };

            fbOptions.Scope.Add("email");
            app.UseFacebookAuthentication(fbOptions);
        }
    }
}