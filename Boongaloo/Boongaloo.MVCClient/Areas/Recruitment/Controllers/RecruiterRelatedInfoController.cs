using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Boongaloo.MVCClient.Areas.Recruitment.Models;

namespace Boongaloo.MVCClient.Areas.Recruitment.Controllers
{
    public class RecruiterRelatedInfoController : Controller
    {
        // GET: Recruitment/RecruiterRelatedInfo
        public ActionResult Index()
        {
            var claimsCollection = ClaimsPrincipal.Current.Claims;

            var user = new Recruiter()
            {
                FirstName = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.GivenName).Value,
                LastName = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.FamilyName).Value,
                Email = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.Email).Value,
                PhoneNumber = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.PhoneNumber).Value,
                SkypeName = claimsCollection.First(claim => claim.Type == "skypename").Value
            };

            return View(user);
        }
    }
}