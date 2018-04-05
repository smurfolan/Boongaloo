using IdentityServer3.Core;
using System;
using System.Linq;
using System.Web.Mvc;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;
using BoongalooCompany.IdentityServer.Services;
using Boongaloo.DTO.Enums;
using System.Collections.Concurrent;

namespace BoongalooCompany.IdentityServer.Controllers
{
    public class CreateUserAccountController : Controller
    {
        private static readonly ConcurrentDictionary<Guid, string> SigninValues = new ConcurrentDictionary<Guid, string>();
        private static readonly ConcurrentDictionary<Guid, CreateUserAccountModel> ContextUsers = new ConcurrentDictionary<Guid, CreateUserAccountModel>();
        private static readonly ConcurrentDictionary<Guid, string> SecretlyGeneratedCodes = new ConcurrentDictionary<Guid, string>();

        private readonly IConfirmationCodeDeliveryService _mailDeliveryService;

        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CreateUserAccountController(/*IMailDeliveryService mailDeliveryService*/)
        {
            // this._mailDeliveryService = mailDeliveryService;
            this._mailDeliveryService = new ConfirmationCodeDeliveryService();
        }

        [HttpGet]
        public ActionResult Index(string signin)
        {
            return View(new CreateUserAccountModel());
        }

        [HttpPost]
        public ActionResult Index(string signin, CreateUserAccountModel model)
        {
            model.TemporaryUserId = Guid.NewGuid();
 
            SigninValues.TryAdd(model.TemporaryUserId, signin);
            ContextUsers.TryAdd(model.TemporaryUserId, model);

            var randomSixDigitNumber = this.RandomSixDigitNumber();
            SecretlyGeneratedCodes.TryAdd(model.TemporaryUserId, randomSixDigitNumber);

            try
            {
                switch (model.ConfirmationType)
                {
                    case ConfirmationTypeEnum.Email:
                        this._mailDeliveryService.SendCodeViaMail(model.Email, randomSixDigitNumber); break;
                    case ConfirmationTypeEnum.Sms:
                        this._mailDeliveryService.SendCodeViaSms(model.PhoneNumber, randomSixDigitNumber); break;
                }
            }
            catch (Exception ex)
            {
                // TODO: Upgrade account in order to be able to send SMS to whoever (https://www.twilio.com/docs/api/errors/21608)
                var unverifiedPhoneNumber = (ex is Twilio.Exceptions.ApiException) 
                    && ((Twilio.Exceptions.ApiException)ex).Code == 21608;

                Log.Error(ex.Message, ex);

                SigninValues.TryRemove(model.TemporaryUserId, out string signingValueBeingRemoved);
                ContextUsers.TryRemove(model.TemporaryUserId, out CreateUserAccountModel contextUserBeingRemoved);

                var errorMessage = unverifiedPhoneNumber 
                    ? "Sorry, your phone number is not supported. Try email." 
                    : "Impossible to send you confirmation code.";

                ModelState.AddModelError("ConfirmationType", errorMessage);
                return View("~/Views/CreateUserAccount/Index.cshtml", model);
            }

            return Json(Url.Action("ConfirmationCodeInput", "CreateUserAccount", new ConfirmationCodeInputModel()
            {
                TemporaryUserId = model.TemporaryUserId,
                Signin = signin
            }));
        }

        [HttpGet]
        public ActionResult ConfirmationCodeInput(Guid temporaryUserId, string signin)
        {
            return View("ConfirmationCodeInput", new ConfirmationCodeInputModel()
            {
                TemporaryUserId = temporaryUserId,
                Signin = signin
            });
        }

        [HttpPost]
        public ActionResult SubmitConfirmationCode(ConfirmationCodeInputModel confirmationCode)
        {
            string confirmationCodeSentToUser;
            SecretlyGeneratedCodes.TryGetValue(confirmationCode.TemporaryUserId, out confirmationCodeSentToUser);

            if (confirmationCode.Code == confirmationCodeSentToUser)
            {
                SecretlyGeneratedCodes.TryRemove(confirmationCode.TemporaryUserId, out string codeBeingRemoved);
                return this.CreateUserAndNavigateToLoginPage(confirmationCode.TemporaryUserId);
            }

            ModelState.AddModelError("Code", "Code you've entered is wrong.");

            return View("ConfirmationCodeInput", new ConfirmationCodeInputModel()
            {
                TemporaryUserId = confirmationCode.TemporaryUserId,
                Signin = SigninValues.FirstOrDefault(sv => sv.Key == confirmationCode.TemporaryUserId).Value
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
                ClaimValue = model.Username
            });
        }

        /// <summary>
        /// Method is used to save the user who has confirmed by sending the code received in his email box and navigate to login page.
        /// </summary>
        /// <param name="confirmationCodeUserEmail"></param>
        /// <returns></returns>
        private ActionResult CreateUserAndNavigateToLoginPage(Guid confirmationCodeTemporaryUserId)
        {
            var contextUser = ContextUsers.FirstOrDefault(u => u.Value.TemporaryUserId == confirmationCodeTemporaryUserId);
            var signingValue = SigninValues.FirstOrDefault(sv => sv.Key == confirmationCodeTemporaryUserId).Value;

            using (var userRepository = new UserRepository())
            {
                var newUser = new User
                {
                    Subject = Guid.NewGuid().ToString(),
                    IsActive = true
                };

                this.AddUserClaimsForLocalUser(newUser, contextUser.Value);

                userRepository.AddUser(newUser);

                ContextUsers.TryRemove(contextUser.Key, out CreateUserAccountModel contextUserBeingRemoved);
                SigninValues.TryRemove(confirmationCodeTemporaryUserId, out string signingValueBeingRemoved);

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