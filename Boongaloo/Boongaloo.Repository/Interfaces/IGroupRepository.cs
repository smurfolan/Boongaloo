using System;
using System.Collections.Generic;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetGroups();
        GroupResponseDto GetGroupById(int groupId);
        IEnumerable<Group> GetGroupsForUserId(int userId);
        IEnumerable<Group> GetGroupsForAreaId(int areaId); 

        void InsertGroup(Group groupToInsert);
        void DeleteGroup(int groupId);
        void UpdateGroup(Group updatedGroup);

        // Consider moving these away when DbContext is changes. We keep these methods because we are artificially maintaining RDB
        // using JSON files.
        int InsertGroup(Group grouToInsert, IEnumerable<int> areaIds, IEnumerable<int> tagIds, IEnumerable<int> userIds);
        IEnumerable<Group> GetGroups(double latitude, double longitude);

        void Save();
    }
}
