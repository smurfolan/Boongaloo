using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Boongaloo.DTO.Applicant;
using Boongaloo.MVCClient.Areas.Application.Models;

namespace Boongaloo.MVCClient.Areas.Application.Controllers
{
    public class ApplicantRelatedInfoController : Controller
    {
        // GET: Application/ApplicantRelatedInfo
        public ActionResult Index()
        {
            var claimsCollection = ClaimsPrincipal.Current.Claims;

            var user = new Applicant()
            {
                FirstName = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.GivenName).Value,
                LastName = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.FamilyName).Value,
                Email = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.Email).Value,
                PhoneNumber = claimsCollection.First(claim => claim.Type == IdentityModel.JwtClaimTypes.PhoneNumber).Value,
                SkypeName = claimsCollection.First(claim => claim.Type == "skypename").Value
            };

            return View(user);
        }

        [HttpPost]
        public ActionResult SubmitAdditionalUserData(UserData userInfo)
        {

            return null;
        }
    }
}