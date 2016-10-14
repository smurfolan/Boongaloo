using System;
using System.Collections.Generic;
using System.Device.Location;/*Consider moving this away and replacing it with own calculations class.*/
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
            toBeUpdated.Tags = updatedGroup.Tags;
        }

        // Consider moving these away when DbContext is changes. We keep these methods because we are artificially maintaining RDB
        // using JSON files. We manage our ow bridge tables and maintain them by our ow which is not a good practice.
        public void InsertGroup(Group grouToInsert, IEnumerable<int> areaIds)
        {
            var latestGroupRecord = this.GetGroups().OrderBy(x => x.Id).LastOrDefault();
            var nextGroupId = latestGroupRecord?.Id + 1 ?? 1;

            grouToInsert.Id = nextGroupId;
            
            // Insert into the bridge table AreaGroup
            foreach (var areaId in areaIds)
            {
                var latestAreaToGroupRecord = this._dbContext.AreaToGroup.OrderBy(x => x.Id).LastOrDefault();
                var nextAreaToGroupId = latestAreaToGroupRecord?.Id + 1 ?? 1;
                
                this._dbContext.AreaToGroup.Add(new AreaToGroup()
                {
                    AreaId = areaId,
                    GroupId = grouToInsert.Id,
                    Id = nextAreaToGroupId
                });
            }

            // Insert the new group
            this._dbContext.Groups.Add(new Group()
            {
                Id = grouToInsert.Id,
                Name = grouToInsert.Name,
                Tags = grouToInsert.Tags
            });
        }
        public IEnumerable<Group> GetGroups(double latitude, double longitude)
        {
            var currentUserLocation = new GeoCoordinate(latitude, longitude);

            var areasInsideOfWhichUserIsCurrentlyIn = this._dbContext.Areas
                .Where(x => currentUserLocation.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)) <= (int)x.Radius)
                .Select(y => y.Id);

            var groupIds =
                this._dbContext.AreaToGroup.Where(x => areasInsideOfWhichUserIsCurrentlyIn.Contains(x.AreaId))
                    .Select(m => m.GroupId);

            return this._dbContext.Groups.Where(x => groupIds.Contains(x.Id));
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
