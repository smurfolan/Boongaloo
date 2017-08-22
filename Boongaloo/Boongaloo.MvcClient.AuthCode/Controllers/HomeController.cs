using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using IdentityModel.Client;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class HomeController : Controller
    {
        private ObjectCache _cache;
        private readonly string tokensCacheKey = "Tokens";

        public HomeController()
        {
            _cache = MemoryCache.Default;
        }

        // GET: Home
        public ActionResult Index()
        {
            var authorizeRequest = new AuthorizeRequest(Constants.BoongalooSTSAuthorizationEndpoint);

            var state = HttpContext.Request.Url.OriginalString;

            var url = authorizeRequest.CreateAuthorizeUrl(
            "tripgalleryauthcode",
            "code",
            "openid profile address boongaloomanagement offline_access",
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
            Constants.BoongalooClientSecret
            );

            var tokenResponse = await client.RequestAuthorizationCodeAsync(
            authCode,
            Constants.BoongalooMvcAuthCodePostLogoutCallback
            );

            this._cache[this.tokensCacheKey] = new TokenModel()
            {
                AccessToken = tokenResponse.AccessToken,
                IdToken = tokenResponse.IdentityToken,
                RefreshToken = tokenResponse.RefreshToken,
                AccessTokenExpiresAt = DateTime.Parse(DateTime.Now.AddSeconds(tokenResponse.ExpiresIn).ToString(CultureInfo.InvariantCulture), CultureInfo.InstalledUICulture)
            };

            return View();
        }

        public ActionResult StartCallingWebApi()
        {
            var timer = new Timer(async (e) =>
            {
                var cachedStuff = this._cache.Get(this.tokensCacheKey) as TokenModel;
                await ExecuteWebApiCall(cachedStuff);
            }, null, 0, Convert.ToInt32(TimeSpan.FromMinutes(2).TotalMilliseconds));
            
            return null;
        }

        private async Task ExecuteWebApiCall(TokenModel cachedStuff)
        {
            // Ensure that access token expires in more than one minute
            if (cachedStuff != null && cachedStuff.AccessTokenExpiresAt > DateTime.Now.AddMinutes(1))
            {
                await MakeValidApiCall(cachedStuff);
            }
            else
            {
                // Use the refresh token to get a new access token, id token and refresh token
                var client = new TokenClient(
                    Constants.BoongalooSTSTokenEndpoint,
                    "tripgalleryauthcode",
                    Constants.BoongalooClientSecret
                );

                if (cachedStuff != null)
                {
                    var newTokens = await client.RequestRefreshTokenAsync(cachedStuff.RefreshToken);

                    var value = new TokenModel()
                    {
                        AccessToken = newTokens.AccessToken,
                        IdToken = newTokens.IdentityToken,
                        RefreshToken = newTokens.RefreshToken,
                        AccessTokenExpiresAt =
                            DateTime.Parse(
                                DateTime.Now.AddSeconds(newTokens.ExpiresIn).ToString(CultureInfo.InvariantCulture), CultureInfo.InstalledUICulture)
                    };

                    this._cache.Set(this.tokensCacheKey, (object)value, new CacheItemPolicy());

                    await MakeValidApiCall(value);
                }
            }
        }

        private static async Task MakeValidApiCall(TokenModel cachedStuff)
        {
            var client = new HttpClient();

            client.SetBearerToken(cachedStuff.AccessToken);

            client.BaseAddress = new Uri(Constants.BoongalooStagingAPI);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var areaByIdFromWebApi = await client.GetAsync("/api/v1/areas/fe990448-670a-42fb-8b02-18f24c94cb49");
                Debug.WriteLine($"Call made at: {DateTime.Now} and result was: {areaByIdFromWebApi.StatusCode}");
                Debug.WriteLine($"Token expires at: {cachedStuff.AccessTokenExpiresAt}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while fetching data from WebApi. More:" + ex.Message);
            }
        }
    }

    public class TokenModel
    {
        public string AccessToken { get; set; }

        public string IdToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpiresAt { get; set; }
    }
}