namespace BoongalooCompany.IdentityServer.Services
{
    public interface IConfirmationCodeDeliveryService
    {
        void SendCodeViaMail(string recipientEmail, string code);
        void SendCodeViaSms(string phoneNumber, string code);
    }
}
