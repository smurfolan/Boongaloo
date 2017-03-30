using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdentityModel.Client;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var authorizeRequest = new AuthorizeRequest(Constants.BoongalooSTSAuthorizationEndpoint);

            var state = HttpContext.Request.Url.OriginalString;

            var url = authorizeRequest.CreateAuthorizeUrl(
            "tripgalleryauthcode",
            "code",
            "openid profile address boongaloomanagement offline_access",//"boongaloomanagement",
            Constants.BoongalooMvcAuthCodePostLogoutCallback,
            state);

            HttpContext.Response.Redirect(url);
            return null;
        }

        public async Task<ActionResult> StsCallBackForAuthCodeClient()
        {
            var authCode = Request.QueryString["code"];

            var client = new TokenClient(
            Constants.BoongalooSTSTokenEndpoint,
            "tripgalleryauthcode",
            Constants.BoongalooClientSecret//.Sha256()
            );

            var tokenResponse = await client.RequestAuthorizationCodeAsync(
            authCode,
            Constants.BoongalooMvcAuthCodePostLogoutCallback
            );

            ViewData["access_token"] = tokenResponse.AccessToken;
            ViewData["id_token"] = tokenResponse.IdentityToken;
            ViewData["refresh_token"] = tokenResponse.RefreshToken;
            ViewData["rf_expires_in"] = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString(CultureInfo.InvariantCulture);

            return View();
        }
    }
}