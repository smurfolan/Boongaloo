namespace BoongalooCompany.IdentityServer.Models
{
    public class CreateUserAccountModel : BaseUserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
