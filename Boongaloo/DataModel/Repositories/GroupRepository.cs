using System;
using System.Collections.Generic;

namespace DataModel.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public GroupRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Group> GetGroups()
        {
            throw new NotImplementedException();
        }

        public Group GetGroupById(int groupId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Group> GetGroupsForUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Group> GetGroupsForAreaId(int areaId)
        {
            throw new NotImplementedException();
        }

        public void InsertGroup(Group groupToInsert)
        {
            throw new NotImplementedException();
        }

        public void DeleteGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public void UpdateGroup(Group updatedGroup)
        {
            throw new NotImplementedException();
        }

        public void InsertGroup(Group grouToInsert, IEnumerable<int> areaIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Group> GetGroups(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
