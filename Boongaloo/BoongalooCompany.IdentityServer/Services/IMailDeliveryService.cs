using System.Threading.Tasks;

namespace BoongalooCompany.IdentityServer.Services
{
    public interface IMailDeliveryService
    {
        void SendCode(string recipientEmail, string code);
    }
}
