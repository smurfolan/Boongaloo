using System.Collections.Generic;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<UserResponseDto> GetUsers();
        User GetUserById(int areaId);
        IEnumerable<User> GetUsersForGroupId(int groupId); 

        int InsertUser(NewUserRequestDto area);
        void DeleteUser(int areaId);
        void UpdateUser(NewUserRequestDto area);

        void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<int> groupSubscriptions);

        IEnumerable<UserResponseDto> GetUsersFromGroup(int id);
        IEnumerable<User> GetUsersFromArea(int id);

        IEnumerable<int> GetUserSubscriptions(int uid);

        void Save();
    }
}
