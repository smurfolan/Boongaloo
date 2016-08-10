using IdentityServer3.Core;
using System;
using System.Web.Mvc;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;

namespace BoongalooCompany.IdentityServer.Controllers
{
    public class CreateUserAccountController : Controller
    {
        // GET: CreateUserAccount
        [HttpGet]
        public ActionResult Index(string signin)
        {
            return View(new CreateUserAccountModel());
        }

        [HttpPost]
        public ActionResult Index(string signin, CreateUserAccountModel model)
        {
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

                    this.AddUserClaimsForLocalUser(newUser, model);
         
                    userRepository.AddUser(newUser);

                    // redirect to the login page, passing in the signin parameter
                    return Redirect("~/identity/" + Constants.RoutePaths.Login + "?signin=" + signin);
                }
            }
            return View();
        }

        /// <summary>
        /// Collects all the user provided information into list of claims related to the user in our store
        /// </summary>
        /// <param name="newUser">The user that is currently being created</param>
        /// <param name="model">User provided information</param>
        private void AddUserClaimsForLocalUser(User newUser, CreateUserAccountModel model)
        {
            // TODO: Encrypt algorithm for passwords.
            newUser.UserName = model.Username;
            newUser.Password = model.Password;

            // EMAIL
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = Constants.ClaimTypes.Email,
                ClaimValue = model.Email
            });

            // GIVEN NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = Constants.ClaimTypes.GivenName,
                ClaimValue = model.FirstName
            });

            // FAMILY NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = Constants.ClaimTypes.FamilyName,
                ClaimValue = model.LastName
            });

            // ROLE
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = Constants.ClaimTypes.Role,
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
                ClaimType = Constants.ClaimTypes.PhoneNumber,
                ClaimValue = model.PhoneNumber
            });
        }
    }
}