using System;
using System.Collections.Generic;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetGroups();
        Group GetGroupById(int groupId);
        IEnumerable<Group> GetGroupsForUserId(int userId);
        IEnumerable<Group> GetGroupsForAreaId(int areaId); 

        void InsertGroup(Group groupToInsert);
        void DeleteGroup(int groupId);
        void UpdateGroup(Group updatedGroup);

        void Save();
    }
}
