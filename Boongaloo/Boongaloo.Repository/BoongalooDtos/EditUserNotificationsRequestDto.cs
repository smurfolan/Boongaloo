using System.Collections.Generic;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class EditUserNotificationsRequestDto
    {
        public bool AutomaticallySubscribeToAllGroups { get; set; }

        public bool AutomaticallySubscribeToAllGroupsWithTag { get; set; }

        public IEnumerable<int> SubscribedTagIds;
    }
}
