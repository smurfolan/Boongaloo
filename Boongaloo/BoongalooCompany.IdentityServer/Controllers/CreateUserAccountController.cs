using IdentityServer3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;
using BoongalooCompany.IdentityServer.Services;
using System.Threading.Tasks;

namespace BoongalooCompany.IdentityServer.Controllers
{
    public class CreateUserAccountController : Controller
    {
        private static readonly Dictionary<string, string> SigninValues = new Dictionary<string, string>();
        private static readonly List<CreateUserAccountModel> ContextUsers = new List<CreateUserAccountModel>();
        private static readonly Dictionary<string, string> SecretlyGeneratedCodes = new Dictionary<string, string>();

        private readonly IMailDeliveryService _mailDeliveryService;

        public CreateUserAccountController(/*IMailDeliveryService mailDeliveryService*/)
        {
            // this._mailDeliveryService = mailDeliveryService;
            this._mailDeliveryService = new MailDeliveryService();
        }

        // GET: CreateUserAccount
        [HttpGet]
        public ActionResult Index(string signin)
        {
            return View(new CreateUserAccountModel());
        }

        [HttpPost]
        public ActionResult Index(string signin, CreateUserAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            SigninValues.Add(model.Email, signin);
            ContextUsers.Add(model);

            // var generate code
            var randomSixDigitNumber = this.RandomSixDigitNumber();
            SecretlyGeneratedCodes.Add(model.Email, randomSixDigitNumber);

            // send it on email. TODO: Currently we silently move the user to login page. Notification could be implemented.
            try
            {
                this._mailDeliveryService.SendCode(model.Email, randomSixDigitNumber);
            }
            catch (Exception)
            {
                SigninValues.Remove(model.Email);
                ContextUsers.Remove(model);

                return Redirect($"/identity/login?signin={signin}");
            }

            // navigate to page where input for th ecode is expected
            return View("ConfirmationCodeInput", new ConfirmationCodeInputModel()
            {
                UserEmail = model.Email,
                Signin = signin
            });
        }

        [HttpPost]
        public ActionResult SubmitConfirmationCode(ConfirmationCodeInputModel confirmationCode)
        {
            string confirmationCodeSentToUser;
            SecretlyGeneratedCodes.TryGetValue(confirmationCode.UserEmail, out confirmationCodeSentToUser);

            if (confirmationCode.Code == confirmationCodeSentToUser)
            {
                SecretlyGeneratedCodes.Remove(confirmationCode.UserEmail);
                return this.CreateUserAndNavigateToLoginPage(confirmationCode.UserEmail);
            }

            ModelState.AddModelError("Code", "Code you've entered is wrong.");

            return View("ConfirmationCodeInput", new ConfirmationCodeInputModel()
            {
                UserEmail = confirmationCode.UserEmail,
                Signin = SigninValues.FirstOrDefault(sv => sv.Key == confirmationCode.UserEmail).Value
            });
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
        /// <param name="confirmationCodeUserEmail"></param>
        /// <returns></returns>
        private ActionResult CreateUserAndNavigateToLoginPage(string confirmationCodeUserEmail)
        {
            var contextUser = ContextUsers.FirstOrDefault(u => u.Email == confirmationCodeUserEmail);
            var signingValue = SigninValues.FirstOrDefault(sv => sv.Key == confirmationCodeUserEmail).Value;

            using (var userRepository = new UserRepository())
            {
                var newUser = new User
                {
                    Subject = Guid.NewGuid().ToString(),
                    IsActive = true
                };

                this.AddUserClaimsForLocalUser(newUser, contextUser);

                userRepository.AddUser(newUser);

                ContextUsers.Remove(contextUser);
                SigninValues.Remove(confirmationCodeUserEmail);

                // redirect to the login page, passing in the signin parameter
                return Redirect("~/identity/" + Constants.RoutePaths.Login + "?signin=" + signingValue);
            }
        }

        /// <summary>
        /// Used when generating a code to be sent for confirmation by the user.
        /// </summary>
        /// <returns>Random six digit number.</returns>
        private string RandomSixDigitNumber()
        {
            var r = new Random();
            var randNum = r.Next(1000000);
            var sixDigitNumber = randNum.ToString("D6");

            return sixDigitNumber;
        }
    }
}