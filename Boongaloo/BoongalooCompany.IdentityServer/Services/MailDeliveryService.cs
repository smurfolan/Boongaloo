using System.Net;
using System.Net.Mail;
using System.Text;

namespace BoongalooCompany.IdentityServer.Services
{
    public class MailDeliveryService : IMailDeliveryService
    {
        private SmtpClient _smtpClient;

        public MailDeliveryService()
        {
            _smtpClient = new SmtpClient()
            {
                Host = "Smtp.Gmail.com",
                Port = 587,
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("boongalooapplication@gmail.com", "BoongalooSouthPark")
            };    
        }
        public void SendCode(string recipientEmail, string code)
        {
            _smtpClient.Send(new MailMessage()
            {
                From = new MailAddress("boongalooapplication@gmail.com", "Boongaloo"),
                To = { recipientEmail },
                Subject = "Confirmation code for new registration",
                Body = $"Your confirmation code is: {code}",
                BodyEncoding = Encoding.UTF8
            });
        }
    }
}