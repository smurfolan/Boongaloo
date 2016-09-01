using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;

namespace Boongaloo.Repository.Repositories
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        private GroupContext _groupContext;

        private bool disposed = false;

        public GroupRepository(GroupContext groupContext)
        {
            _groupContext = groupContext;
        }

        public IEnumerable<Group> GetGroups()
        {
            return this._groupContext.Groups;
        }

        public Group GetGroupById(int groupId)
        {
            return this._groupContext.Groups.FirstOrDefault(x => x.Id == groupId);
        }

        public void InsertGroup(Group groupToInsert)
        {
            this._groupContext.Groups.Insert(0, groupToInsert);

            Save();
        }

        public void DeleteGroup(int groupId)
        {
            var groupToBeDeleted = this._groupContext.Groups.FirstOrDefault(x => x.Id == groupId);
            this._groupContext.Groups.Remove(groupToBeDeleted);

            Save();
        }

        public void UpdateGroup(Group updatedGroup)
        {
            var toBeUpdated = this._groupContext.Groups.FirstOrDefault(x => x.Id == updatedGroup.Id);

            if (toBeUpdated == null)
                return;

            toBeUpdated.Name = updatedGroup.Name;
            toBeUpdated.Radius = updatedGroup.Radius;
            toBeUpdated.Tags = updatedGroup.Tags;
            toBeUpdated.Users = updatedGroup.Users;

            Save();
        }

        public void Save()
        {
            this._groupContext.SaveChanges();
        }

        

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._groupContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
