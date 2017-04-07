namespace BoongalooCompany.Repository.Entities
{
    public class RefreshToken
    {
        public string Key { get; set; }
        public IdentityServer3.Core.Models.RefreshToken Token { get; set; }

        public RefreshToken()
        {
            Token = new IdentityServer3.Core.Models.RefreshToken();
        }
    }
}
