using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Boongaloo.MVCClient;
using Boongaloo.MVCClient.Helpers;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Boongaloo;
using Microsoft.Owin.Security.Notifications;

[assembly: OwinStartup(typeof(Startup))]
namespace Boongaloo.MVCClient
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = 
                IdentityModel.JwtClaimTypes.Name;

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                // How much the cookie will remain valid from the point it is created.
                ExpireTimeSpan = new TimeSpan(0, 30, 0),
                // Tells the middleware to issue a new cookie with a new expiration time after half of the expiration window has passed.
                SlidingExpiration = true
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {

                ClientId = "boongaloohybrid",
                Authority = Constants.BoongalooSTS,
                RedirectUri = Constants.BoongalooMVC,
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "openid profile address boongaloomanagement offline_access",
                // identity_token lifetime won't be used, but the expiration options of the Authentication ticket will be used.
                UseTokenLifetime = false, 
                PostLogoutRedirectUri = Constants.BoongalooMVC,

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    SecurityTokenValidated = async n =>
                    {
                        TokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        TokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);

                        // TODO: Claims transformations and store user in DB if not available

                        // CLAIMS TRANSFORMATION
                        await ClaimsTransformations.AssignClaimsReceivedFromIdentityServer(n, n.ProtocolMessage.AccessToken);

                        // DB storing
                        // TODO

                        await Task.FromResult(0);
                    },
                    // Fires whenever the middleware has to redirect to the identity provider(In our case, STS).
                    RedirectToIdentityProvider = async n =>
                    {
                        // get id token to add as id token hint
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var identityTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (identityTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = identityTokenHint.Value;
                            }
                        }
                        await Task.FromResult(0);
                    }
                }
            });

        }      
    }
}
