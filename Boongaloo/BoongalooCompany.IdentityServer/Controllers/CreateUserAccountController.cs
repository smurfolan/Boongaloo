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
        private string _signinValue;
        private CreateUserAccountModel _contextUser = new CreateUserAccountModel();
        private string _secretlyGeneratedCode;

        // GET: CreateUserAccount
        [HttpGet]
        public ActionResult Index(string signin)
        {
            this._signinValue = signin;
            return View(_contextUser);
        }

        [HttpPost]
        public ActionResult Index(string signin, CreateUserAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            this._signinValue = signin;
            this._contextUser = model;

            // var generate code
            this._secretlyGeneratedCode = "123456"; // TODO: Make it dynamic.
            // send it on email
            // navigate to page where input for th ecode is expected

            return View("ConfirmationCodeInput");
        }

        [HttpPost]
        public ActionResult SubmitConfirmationCode(ConfirmationCodeInputModel confirmationCode)
        {
            return null;
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
        }

        /// <summary>
        /// Method is used to save the user who has confirmed by sending the code received in his email box and navigate to login page.
        /// </summary>
        /// <returns></returns>
        private ActionResult CreateUserAndNavigateToLoginPage()
        {
            using (var userRepository = new UserRepository())
            {
                var newUser = new User
                {
                    Subject = Guid.NewGuid().ToString(),
                    IsActive = true
                };

                this.AddUserClaimsForLocalUser(newUser, _contextUser);

                userRepository.AddUser(newUser);

                // redirect to the login page, passing in the signin parameter
                return Redirect("~/identity/" + Constants.RoutePaths.Login + "?signin=" + _signinValue);
            }
        }
    }
}