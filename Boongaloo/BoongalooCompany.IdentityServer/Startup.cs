using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.Default;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Services;
using BoongalooCompany.IdentityServer.Config;
using BoongalooCompany.IdentityServer.Helpers;
using BoongalooCompany.IdentityServer.Services;
using Serilog;

namespace BoongalooCompany.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Trace()
            .CreateLogger();

            app.Map("/identity", idsrvApp =>
            {
                var corsPolicyService = new DefaultCorsPolicyService()
                {
                    AllowAll = true
                };

                var idServerServiceFactory = new IdentityServerServiceFactory()
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get());

                var defaultViewServiceOptions = new DefaultViewServiceOptions
                {
                    CacheViews = false
                };
                defaultViewServiceOptions.Stylesheets.Add("/Content/Site.css");

                idServerServiceFactory.ConfigureDefaultViewService(defaultViewServiceOptions);

                idServerServiceFactory.CorsPolicyService = new
                    Registration<IdentityServer3.Core.Services.ICorsPolicyService>(corsPolicyService);

                // use custom user service
                var customUserService = new CustomUserService();
                idServerServiceFactory.UserService = new Registration<IUserService>(resolver => customUserService);

                // use custom service for tokens maintainance
                var customRefreshTokenStore = new CustomRefreshTokenStore();
                idServerServiceFactory.RefreshTokenStore = new Registration<IRefreshTokenStore>(resolver => customRefreshTokenStore);

                var options = new IdentityServerOptions
                {
                    Factory = idServerServiceFactory,
                    SiteName = "STS",
                    SigningCertificate = LoadCertificate(),
                    IssuerUri = Boongaloo.Constants.BoongalooIssuerUri,
                    PublicOrigin = Boongaloo.Constants.BoongalooSTSOrigin,

                    AuthenticationOptions = new AuthenticationOptions()
                    {
                        EnablePostSignOutAutoRedirect = true,
                        // PostSignOutAutoRedirectDelay = 2,
                        // Provide link to other(additional) web page in order to be able to login (Registration, Reset password etc.)
                        LoginPageLinks = new List<LoginPageLink>()
                        {
                            new LoginPageLink()
                            {
                                Type = "createaccount"/*Should be unique*/,
                                Text = "Crate a new account",
                                Href = "~/createuseraccount"
                            }
                        },
                        IdentityProviders = this.ConfigureIdentityProviders
                    },
                    CspOptions = new CspOptions()
                    {
                        Enabled = false
                        // once available, leave enabled at true and use:
                        // FrameSrc = "https://localhost:44318 https://localhost:44316"
                        // or
                        // FrameSrc = "*" for all URIs
                    },
                    LoggingOptions = new LoggingOptions()
                    {
                        //EnableHttpLogging = true, https://github.com/IdentityServer/IdentityServer3/issues/1672
                        EnableWebApiDiagnostics = true
                    }

                };

                idsrvApp.UseIdentityServer(options);
            });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\certificates\idsrv3test.pfx",
                AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

        private void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            ExternalAuthHelper.ConfigureExternalAuthProviders(app, signInAsType);
        }

        
    }
}
