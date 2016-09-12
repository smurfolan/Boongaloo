using AutoMapper;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boongaloo.API.Helpers;
using System.Web.Http.Cors;
using IdentityServer3.AccessTokenValidation;
using System.IdentityModel.Tokens;

namespace Boongaloo.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            // ensures that the api is only accessible if the access token 
            // provided by the TripGallery STS contains the gallerymanagement scope inside of it.
            // It happens in combination with the [Authorize] attribute on controller level.
            app.UseIdentityServerBearerTokenAuthentication(
             new IdentityServerBearerTokenAuthenticationOptions
             {
                 Authority = Constants.BoongalooSTS,
                 RequiredScopes = new[] { "boongaloomanagement" }
             });

            var config = WebApiConfig.Register();

            app.UseWebApi(config);

            InitAutoMapper();
        }

        private void InitAutoMapper()
        {
            // Here we have to make the mapping between entities and DTOs

            Mapper.AssertConfigurationIsValid();
        }
    }
}
