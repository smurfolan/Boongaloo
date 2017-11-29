using Boongaloo.DTO.Enums;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using FluentValidation;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Lookups.V1;
using Twilio.Types;
using System.Configuration;
using System;

namespace BoongalooCompany.IdentityServer.Validators
{
    public class CreateUserAccountModelValidator : AbstractValidator<CreateUserAccountModel>
    {
        private readonly string TwilioSID;
        private readonly string TwilioAuthToken;

        public CreateUserAccountModelValidator()
        {
            this.TwilioSID = ConfigurationManager.AppSettings["TwilioSID"];
            this.TwilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];

            RuleFor(account => account.Password)
                .Length(6,20)
                .WithMessage("Password length must be 6-20 symbols long");

            RuleFor(account => account.Email)
                .EmailAddress();

            RuleFor(account => account.Username)
                .Must(BeNonExistingUsername)
                .WithMessage("Username already exists");

            RuleFor(account => account.ConfirmationType).Must((account, confirmationType) =>
                 MustBeValidConfirmationData(account.PhoneNumber, account.Email, confirmationType))
                .WithMessage("Invalid verification info.");
        }

        private bool MustBeValidConfirmationData(
            string phoneNumber, 
            string email, 
            ConfirmationTypeEnum confirmationType)
        {
            switch (confirmationType)
            {
                case ConfirmationTypeEnum.Email: return this.ValidateEmail(email);
                case ConfirmationTypeEnum.Sms: return this.ValidatePhoneNumber(phoneNumber);
            }
            
            return false;
        }

        private bool ValidatePhoneNumber(string phoneNumber)
        {
            TwilioClient.Init(TwilioSID, TwilioAuthToken);

            try
            {
                var phoneNumberValidationResult = PhoneNumberResource.Fetch(new PhoneNumber(phoneNumber));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private bool ValidateEmail(string email)
        {
            // https://stackoverflow.com/questions/5342375/regex-email-validation
            var regexTemplate = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                        + "@"
                        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            Regex regex = new Regex(regexTemplate);
            Match match = regex.Match(email);

            var emailFormatIsValid = match.Success;
            bool emailIsNotAlreadyExisting;

            using (var userRepository = new UserRepository())
            {
                emailIsNotAlreadyExisting = userRepository.GetUserByEmail(email) == null;
            }
            
            return emailFormatIsValid && emailIsNotAlreadyExisting;
        }

        private bool BeNonExistingUsername(string username)
        {
            using (var userRepository = new UserRepository())
            {
                return userRepository.GetUserByUsername(username) == null;
            }
        }
    }
}