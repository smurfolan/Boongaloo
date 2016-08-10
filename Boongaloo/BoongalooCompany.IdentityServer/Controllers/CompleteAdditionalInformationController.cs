using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.Core.Extensions;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;

namespace BoongalooCompany.IdentityServer.Controllers
{
    public class CompleteAdditionalInformationController : Controller
    {
        // GET: CompleteAdditionalInformation
        public async Task<ActionResult> Index(string provider)
        {
            // we're only allowed here when we have a partial sign-in
            var ctx = Request.GetOwinContext();
            var partialSignInUser = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partialSignInUser == null)
            {
                return View("No partially signed-in user found.");
            }

            return View(new CompleteAdditionalInformationModel() {ExternalProvider = provider});
        }

        [HttpPost]
        public async Task<ActionResult> Index(CompleteAdditionalInformationModel model)
        {
            // we're only allowed here when we have a partial sign-in
            var ctx = Request.GetOwinContext();
            var partialSignInUser = await ctx.Environment.GetIdentityServerPartialLoginAsync();
            if (partialSignInUser == null)
            {
                return View("No partially signed-in user found.");
            }

            if (ModelState.IsValid)
            {
                using (var userRepository = new UserRepository())
                {
                    // create a new account
                    var newUser = new User();
                    newUser.Subject = Guid.NewGuid().ToString();
                    newUser.IsActive = true;

                    // add the external identity provider as login provider
                    // => external_provider_user_id contains the id/key
                    newUser.UserLogins.Add(new UserLogin()
                    {
                        Subject = newUser.Subject,
                        LoginProvider = model.ExternalProvider.ToLower(),
                        ProviderKey = partialSignInUser.Claims.First(c => c.Type == "external_provider_user_id").Value
                    });

                    AddUserClaimsForExternalUser(model, newUser, partialSignInUser);

                    userRepository.AddUser(newUser);

                    // continue where we left off                
                    return Redirect(await ctx.Environment.GetPartialLoginResumeUrlAsync());
                }
            }

            return View();
        }

        /// <summary>
        /// Collects all the user provided information into list of claims related to the user in our store
        /// </summary>
        /// <param name="newUser">The user that logged in from an external account</param>
        /// <param name="model">User provided information</param>
        /// <param name="partialSignInUser">The user who is currently partially logged in.</param>
        private static void AddUserClaimsForExternalUser(
            CompleteAdditionalInformationModel model, 
            User newUser,
            ClaimsIdentity partialSignInUser)
        {
            // EMAIL
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.Email,
                ClaimValue = partialSignInUser.Claims.First(
                    c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.Email).Value
            });

            // GIVEN NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.GivenName,
                ClaimValue = partialSignInUser.Claims.First(c => c.Type == "given_name").Value
            });

            // FAMILY NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.FamilyName,
                ClaimValue = partialSignInUser.Claims.First(c => c.Type == "family_name").Value
            });

            // we could use the access token to obtain user info from linkedin
            //if (model.ExternalProvider.ToLower() == "linkedin")
            //{
            //    newUser.UserClaims.Add(new UserClaim()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Subject = newUser.Subject,
            //        ClaimType = IdentityServer3.Core.Constants.ClaimTypes.AccessTokenHash,
            //        ClaimValue = partialSignInUser.Claims.First(c => c.Type == "urn:linkedin:accesstoken").Value
            //    });
            //}                

            // ROLE
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.Role,
                ClaimValue = model.Role
            });

            // SKYPE NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = "skypename",
                ClaimValue = model.SkypeName
            });

            // PHONE NUMBER
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.PhoneNumber,
                ClaimValue = model.PhoneNumber
            });

            // EXTERNAL PROVIDER
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = "externalprovider",
                ClaimValue = model.ExternalProvider
            });
        }
    }
}