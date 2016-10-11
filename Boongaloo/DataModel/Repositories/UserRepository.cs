using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModel.Repositories
{
    public class UserRepository : IUserRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public UserRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<User> GetUsers()
        {
            return this._dbContext.Users;
        }

        public User GetUserById(int areaId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersForGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public void InsertUser(User user)
        {
            this._dbContext.Users.Add(user);
        }

        public void DeleteUser(int areaId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User area)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersFromGroup(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersFromArea(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
