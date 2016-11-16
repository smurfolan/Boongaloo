using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace BoongalooCompany.IdentityServer.Config
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
             {                
                new Client 
                {
                    ClientId = "tripgalleryauthcode",
                    ClientName = "Trip Gallery (Authorization Code)",
                    Flow = Flows.AuthorizationCode, 
                    AllowAccessToAllScopes = true,
                    RequireConsent = false,

                    // redirect = URI of our callback controller in the MVC application
                    RedirectUris = new List<string>
                    {
                        Boongaloo.Constants.BoongalooIOSCallback
                    },           

                     ClientSecrets = new List<Secret>()
                    {
                        new Secret(Boongaloo.Constants.BoongalooClientSecret.Sha256())
                    },

                    // refresh token options
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    SlidingRefreshTokenLifetime = 1296000,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                } ,
                new Client 
                {
                    ClientId = "tripgalleryimplicit",
                    ClientName = "Trip Gallery (Implicit)",
                    Flow = Flows.Implicit, 
                    AllowAccessToAllScopes = true,

                    IdentityTokenLifetime = 10,
                    AccessTokenLifetime = 120,
                    // If we want to have SSO between Angular app and MVC app we need to have this option set to
                    // false for both the flows they implement(hybrid and implicit).
                    RequireConsent = false,

                    // redirect = URI of the Angular application
                    RedirectUris = new List<string>
                    {
                        Boongaloo.Constants.BoongalooAngular + "callback.html",
                        // for silent refresh
                        Boongaloo.Constants.BoongalooAngular + "silentrefreshframe.html"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        Boongaloo.Constants.BoongalooAngular  + "index.html"
                    }
                },
                new Client 
                {
                    ClientId = "boongaloohybrid",
                    ClientName = "Boongaloohybrid (Hybrid)",
                    Flow = Flows.Hybrid, 
                    AllowAccessToAllScopes = true,
                    // If we want to have SSO between Angular app and MVC app we need to have this option set to
                    // false for both the flows they implement(hybrid and implicit).
                    RequireConsent = false,

                    IdentityTokenLifetime = 10,
                    AccessTokenLifetime = 120,

                    // redirect = URI of the MVC application
                    RedirectUris = new List<string>
                    {
                        Boongaloo.Constants.BoongalooMVC
                    },
                    
                    // Needed when requesting refresh tokens
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret(Boongaloo.Constants.BoongalooClientSecret.Sha256())
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        Boongaloo.Constants.BoongalooMVC
                    }
                }  

             };
        }
    }
}
