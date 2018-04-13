using Microsoft.Owin.Security.Facebook;

namespace BoongalooCompany.IdentityServer.Helpers
{
    public class FacebookAuthenticationProviderWrapper : FacebookAuthenticationProvider
    {
        public override void ApplyRedirect(FacebookApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri + "&display=popup");
        }
    }
}