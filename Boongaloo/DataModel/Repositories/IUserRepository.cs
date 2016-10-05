using System.Collections.Generic;

namespace DataModel.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int areaId);
        IEnumerable<User> GetUsersForGroupId(int groupId);

        void InsertUser(User area);
        void DeleteUser(int areaId);
        void UpdateUser(User area);

        //void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<GroupSubscriptionDto> groupSubscriptions);

        IEnumerable<User> GetUsersFromGroup(int id);
        IEnumerable<User> GetUsersFromArea(int id);

        void Save();
    }
}
