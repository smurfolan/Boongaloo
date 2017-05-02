using BoongalooCompany.IdentityServer.Validators;
using FluentValidation.Attributes;

namespace BoongalooCompany.IdentityServer.Models
{
    [Validator(typeof(CreateUserAccountModelValidator))]
    public class CreateUserAccountModel : BaseUserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
