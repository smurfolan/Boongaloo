﻿using System.Collections.Generic;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.BoongalooDtos;
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

        void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<GroupSubscriptionDto> groupSubscriptions);

        IEnumerable<UserResponseDto> GetUsersFromGroup(int id);
        IEnumerable<User> GetUsersFromArea(int id);

        void Save();
    }
}
