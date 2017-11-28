using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BoongalooCompany.IdentityServer.Services
{
    public class ConfirmationCodeDeliveryService : IConfirmationCodeDeliveryService
    {
        private SmtpClient _smtpClient;
        private readonly string BoongalooEmail;
        private readonly string BoongalooEmailPassword;
        private readonly string SmtpClientHost;

        private readonly string TwilioSID;
        private readonly string TwilioAuthToken;
        private readonly string TwilioOutgoingPhoneNumber;


        public ConfirmationCodeDeliveryService()
        {
            this.BoongalooEmail = ConfigurationManager.AppSettings["BoongalooEmail"];
            this.BoongalooEmailPassword = ConfigurationManager.AppSettings["BoongalooEmailPassword"];
            this.SmtpClientHost = ConfigurationManager.AppSettings["SmtpClientHost"];
            this.TwilioSID = ConfigurationManager.AppSettings["TwilioSID"];
            this.TwilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            this.TwilioOutgoingPhoneNumber = ConfigurationManager.AppSettings["TwilioOutgoingPhoneNumber"];
            
        }
        public void SendCodeViaMail(string recipientEmail, string code)
        {
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

            _smtpClient.Send(new MailMessage()
            {
                From = new MailAddress(BoongalooEmail, "Boongaloo"),
                To = { recipientEmail },
                Subject = "Confirmation code for new registration",
                Body = $"Your confirmation code is: {code}",
                BodyEncoding = Encoding.UTF8
            });
        }

        public void SendCodeViaSms(string phoneNumber, string code)
        {
            TwilioClient.Init(this.TwilioSID, this.TwilioAuthToken);

            var to = new PhoneNumber(phoneNumber);
            var message = MessageResource.Create(
                to,
                from: new PhoneNumber(this.TwilioOutgoingPhoneNumber),
                body: $"Boongaloo verification code: {code}");

            if (message.Status == MessageResource.StatusEnum.Failed || 
                    message.Status == MessageResource.StatusEnum.Undelivered)
                throw new System.Exception($"Error while trying to send confirmation code {code} to {to}");
        }
    }
}