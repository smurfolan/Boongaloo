using Boongaloo.DTO.Enums;
using BoongalooCompany.IdentityServer.Validators;
using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace BoongalooCompany.IdentityServer.Models
{
    [Validator(typeof(CreateUserAccountModelValidator))]
    public class CreateUserAccountModel : BaseUserInfo
    {
        public Guid TemporaryUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmationPassword { get; set; }
        public ConfirmationTypeEnum ConfirmationType { get; set; }
    }
}
