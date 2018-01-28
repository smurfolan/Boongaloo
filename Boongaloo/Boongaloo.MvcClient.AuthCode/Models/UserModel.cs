using Boongaloo.DTO.Enums;
using System;

namespace Boongaloo.MvcClient.AuthCode.Models
{
    public class UserModel
    {
        public UserModel(){}

        public Guid RestId { get; set; }
        public string StsId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public GenderEnum Gender { get; set; }
        public Guid[] Languages { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public NotificationSettingsModel NotificationsSettings { get; set; }
        public SocialLinksModel SocialAccounts { get; set; }
    }
}
