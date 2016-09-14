using System.Collections.Generic;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int areaId);
        IEnumerable<User> GetUsersForGroupId(int groupId); 

        void InsertUser(User area);
        void DeleteUser(int areaId);
        void UpdateUser(User area);

        void SubscribeUserForGroups(int userId, IEnumerable<int> groupIds);
        void UnsubscribeUserFromGroups(int userId, IEnumerable<int> groupIds);

        void Save();
    }
}
