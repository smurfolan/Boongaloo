    using System;
using System.Collections.Generic;
using System.Linq;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;

namespace Boongaloo.Repository.Repositories
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        private readonly BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public GroupRepository(BoongalooDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Group> GetGroups()
        {
            return this._dbContext.Groups;
        }
        public Group GetGroupById(int groupId)
        {
            return this._dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
        }
        public IEnumerable<Group> GetGroupsForUserId(int userId)
        {
            return (from item1 in _dbContext.GroupToUser
                    join item2 in _dbContext.Groups
                    on item1.GroupId equals item2.Id
                    where item1.UserId == userId
                    select item2).ToList();
        }
        public IEnumerable<Group> GetGroupsForAreaId(int areaId)
        {
            return (from item1 in _dbContext.AreaToGroup
                    join item2 in _dbContext.Groups
                    on item1.GroupId equals item2.Id
                    where item1.AreaId == areaId
                    select item2).ToList();
        }

        public void InsertGroup(Group groupToInsert)
        {
            this._dbContext.Groups.Insert(0, groupToInsert);
        }
        public void DeleteGroup(int groupId)
        {
            var groupToBeDeleted = this._dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
            this._dbContext.Groups.Remove(groupToBeDeleted);
        }
        public void UpdateGroup(Group updatedGroup)
        {
            var toBeUpdated = this._dbContext.Groups.FirstOrDefault(x => x.Id == updatedGroup.Id);

            if (toBeUpdated == null)
                return;

            toBeUpdated.Name = updatedGroup.Name;
            toBeUpdated.Radius = updatedGroup.Radius;
            toBeUpdated.Tags = updatedGroup.Tags;
            //toBeUpdated.Users = updatedGroup.Users;
        }

        public void Save()
        {
            this._dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
