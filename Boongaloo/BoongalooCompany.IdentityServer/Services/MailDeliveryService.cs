using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BoongalooCompany.IdentityServer.Services
{
    public class MailDeliveryService : IMailDeliveryService
    {
        private SmtpClient _smtpClient;
        private readonly string BoongalooEmail;
        private readonly string BoongalooEmailPassword;
        private readonly string SmtpClientHost;
        
        public MailDeliveryService()
        {
            this.BoongalooEmail = ConfigurationManager.AppSettings["BoongalooEmail"];
            this.BoongalooEmailPassword = ConfigurationManager.AppSettings["BoongalooEmailPassword"];
            this.SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"];

            _smtpClient = new SmtpClient()
            {
                Host = SmtpClientHost,
                Port = 587,
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(BoongalooEmail, BoongalooEmailPassword)
            };    
        }
        public void SendCode(string recipientEmail, string code)
        {
            _smtpClient.Send(new MailMessage()
            {
                From = new MailAddress(BoongalooEmail, "Boongaloo"),
                To = { recipientEmail },
                Subject = "Confirmation code for new registration",
                Body = $"Your confirmation code is: {code}",
                BodyEncoding = Encoding.UTF8
            });
        }
    }
}