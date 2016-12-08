using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BoongalooCompany.IdentityServer.Helpers;
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

            return View(new CompleteAdditionalInformationModel() {ExternalProviderName = provider});
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
                    var newUser = new User
                    {
                        Subject = Guid.NewGuid().ToString(),
                        IsActive = true
                    };

                    // add the external identity provider as login provider
                    // => external_provider_user_id contains the id/key
                    newUser.UserLogins.Add(new UserLogin()
                    {
                        Subject = newUser.Subject,
                        LoginProvider = model.ExternalProviderName,
                        ProviderKey = partialSignInUser.Claims.First(c => c.Type == "external_provider_user_id").Value
                    });

                    ClaimsHelper.AddUserClaimsForExternalUser(model, newUser, partialSignInUser.Claims);

                    userRepository.AddUser(newUser);

                    // continue where we left off   
                    return Redirect(await ctx.Environment.GetPartialLoginResumeUrlAsync());
                }
            }

            return View();
        }     
    }
}