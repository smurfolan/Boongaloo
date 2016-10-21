using System.Collections.Generic;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class UserNotificationSettingsResponseDto
    {
        public int Id { get; set; }

        public bool AutomaticallySubscribeToAllGroups { get; set; }

        public bool AutomaticallySubscribeToAllGroupsWithTag { get; set; }

        public IEnumerable<int> SubscribedTagIds;
    }
}
