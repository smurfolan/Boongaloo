using System;

namespace BoongalooCompany.IdentityServer.Models
{
    public class ConfirmationCodeInputModel
    {
        public string Code { get; set; }
        public Guid TemporaryUserId { get; set; }
        public string Signin { get; set; }
    }
}