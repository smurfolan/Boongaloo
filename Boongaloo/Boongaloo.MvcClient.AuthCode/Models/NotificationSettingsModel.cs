using System;

namespace Boongaloo.MvcClient.AuthCode.Models
{
    public class NotificationSettingsModel
    {
        public bool AutomaticallySubscribeToAllGroups { get; set; }
        public bool AutomaticallySubscribeToAllGroupsWithTag { get; set; }
        public Guid[] SubscribedTagIds { get; set; }
    }
}
