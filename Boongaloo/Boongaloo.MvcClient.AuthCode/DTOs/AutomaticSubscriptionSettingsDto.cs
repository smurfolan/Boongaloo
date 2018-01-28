using System;
using System.Collections.Generic;

namespace Boongaloo.MvcClient.AuthCode.DTOs
{
    public class AutomaticSubscriptionSettingsDto
    {
        public string id { get; set; }
        public bool automaticallySubscribeToAllGroups { get; set; }
        public bool automaticallySubscribeToAllGroupsWithTag { get; set; }
        public List<Guid> subscribedTagIds { get; set; }
    }
}