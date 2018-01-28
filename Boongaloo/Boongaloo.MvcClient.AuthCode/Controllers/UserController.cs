using Boongaloo.MvcClient.AuthCode.DTOs;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Mvc;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class UserController : BaseController
    {
        private LikkleApiProxy apiProxy;

        public UserController()
        {
            apiProxy = new LikkleApiProxy(this.Tokens);
        }
        // GET: User
        public ActionResult Index()
        {
            if (this.Tokens == null)
                throw new ArgumentException("No tokens are available.");

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenS = jwtHandler.ReadToken(this.Tokens.AccessToken) as JwtSecurityToken;

            var issuer = tokenS.Claims.First(claim => claim.Type == "iss").Value; ;
            var stsId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{issuer}{stsId}");
            var uniqueStSIdentifier = Convert.ToBase64String(plainTextBytes);

            // Automap to ViewModel
            var user = this.GetUserByStsId(uniqueStSIdentifier);

            return View(user);
        }

        private UserDto GetUserByStsId(string stsId)
        {
            return apiProxy.Get<UserDto>(
                $"users/bystsid/{stsId}", 
                null, 
                null);
        }
    }
}