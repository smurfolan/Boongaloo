using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository;
using FluentValidation;

namespace BoongalooCompany.IdentityServer.Validators
{
    public class CreateUserAccountModelValidator : AbstractValidator<CreateUserAccountModel>
    {
        public CreateUserAccountModelValidator()
        {
            RuleFor(account => account.Password)
                .Length(6,20)
                .WithMessage("Password length is wrong. Must be 6-20 symbols long");

            RuleFor(account => account.Email)
                .EmailAddress();

            RuleFor(account => account.Email)
                .Must(NotBeExistingEmailAlready)
                .WithMessage("User with such email already exists");

            RuleFor(account => account.Username)
                .Must(BeNonExistingUsername)
                .WithMessage("Username already exists");
        }

        private bool BeNonExistingUsername(string username)
        {
            using (var userRepository = new UserRepository())
            {
                return userRepository.GetUserByUsername(username) == null;
            }
        }

        private bool NotBeExistingEmailAlready(string email)
        {
            using (var userRepository = new UserRepository())
            {
                return userRepository.GetUserByEmail(email) == null;
            }
        }
    }
}