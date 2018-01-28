using System;
using System.Collections.Generic;

namespace Boongaloo.MvcClient.AuthCode.DTOs
{
    public class UserDto
    {
        public UserDto(){}

        public string id { get; set; }
        public string idsrvUniqueId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string about { get; set; }
        public int gender { get; set; }
        public DateTime birthDate { get; set; }
        public string phoneNumber { get; set; }
        public List<LanguageDto> languages { get; set; }
        public AutomaticSubscriptionSettingsDto automaticSubscriptionSettings { get; set; }
        public SocialLinksDto socialLinks { get; set; }
    }
}