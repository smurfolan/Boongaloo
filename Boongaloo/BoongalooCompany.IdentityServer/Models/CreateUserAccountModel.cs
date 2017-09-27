using BoongalooCompany.IdentityServer.Validators;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BoongalooCompany.IdentityServer.Models
{
    [Validator(typeof(CreateUserAccountModelValidator))]
    public class CreateUserAccountModel : BaseUserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmationPassword { get; set; }
    }
}
