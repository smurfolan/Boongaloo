using System;
using System.Collections.Generic;
using System.Linq;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;

namespace Boongaloo.Repository.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public UserRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }
        public User GetUserById(int userId)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == userId);
        }
        public IEnumerable<User> GetUsersForGroupId(int groupId)
        {
            return (from item1 in _dbContext.GroupToUser
                    join item2 in _dbContext.Users
                    on item1.UserId equals item2.Id
                    where item1.GroupId == groupId
                    select item2).ToList();
        }

        public void InsertUser(User user)
        {
            user.Id = this.GetUsers().Count() + 1;
            _dbContext.Users.Add(user);
        }
        public void DeleteUser(int userId)
        {
            var userToBeDeleted = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            _dbContext.Users.Remove(userToBeDeleted);
        }
        public void UpdateUser(User user)
        {
            var userToBeUpdated = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToBeUpdated == null)
                return;

            // TODO: Do the update
        }

        public void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<GroupSubscriptionDto> groupSubscriptions)
        {
            foreach (var groupSubscriptionDto in groupSubscriptions)
            {
                if (groupSubscriptionDto.IsSubscribtionRequest)
                {
                    var lastRecord = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextRecordId = lastRecord?.Id + 1 ?? 1;

                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        GroupId = groupSubscriptionDto.GroupId,
                        UserId = userId,
                        Id = nextRecordId
                    });
                }
                else
                {
                    var recordToBeRemoved = _dbContext.GroupToUser
                        .FirstOrDefault(x => x.GroupId == groupSubscriptionDto.GroupId && x.UserId == userId);
                    this._dbContext.GroupToUser.Remove(recordToBeRemoved);
                }
            }
        }


        public IEnumerable<User> GetUsersFromGroup(int groupId)
        {
            var userIds = this._dbContext.GroupToUser.Where(x => x.GroupId == groupId).Select(y => y.UserId);

            return this._dbContext.Users.Where(x => userIds.Contains(x.Id));
        }
        public IEnumerable<User> GetUsersFromArea(int areaId)
        {
            var userIds = (from item1 in _dbContext.AreaToGroup
                join item2 in _dbContext.GroupToUser
                    on item1.GroupId equals item2.GroupId
                where item1.AreaId == areaId
                select item2.UserId).ToList();

            return this._dbContext.Users.Where(x => userIds.Contains(x.Id));
        }

        public void Save()
        {
            throw new NotImplementedException();
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
