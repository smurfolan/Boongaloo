namespace BoongalooCompany.IdentityServer.Models
{
    public class ConfirmationCodeInputModel
    {
        public string Code { get; set; }
        public string UserEmail { get; set; }
        public string Signin { get; set; }
    }
}