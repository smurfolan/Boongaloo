using System.Collections.Generic;

namespace Boongaloo.Repository.Entities
{
    public class UserNotificationSettings
    {
        public int Id { get; set; }

        // Foreign key to Users table
        public int UserId { get; set; }

        public bool AutomaticallySubscribeToAllGroups { get; set; }

        public bool AutomaticallySubscribeToAllGroupsWithTag { get; set; }

        public IEnumerable<int> SubscribedTagIds { get; set; }
    }
}
