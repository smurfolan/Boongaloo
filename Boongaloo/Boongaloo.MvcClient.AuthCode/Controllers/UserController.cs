using Boongaloo.MvcClient.AuthCode.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Mvc;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            if (this.Tokens == null)
                throw new ArgumentException("No tokens are available.");

            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenS = jwtHandler.ReadToken(this.Tokens.AccessToken) as JwtSecurityToken;

            var stsId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            var user = this.GetUserByStsId(stsId);

            return View(user);
        }

        private UserModel GetUserByStsId(string stsId)
        {
            throw new NotImplementedException();
        }
    }
}