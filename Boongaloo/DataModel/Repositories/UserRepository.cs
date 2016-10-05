using System;
using System.Collections.Generic;

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
            throw new NotImplementedException();
        }

        public User GetUserById(int areaId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersForGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public void InsertUser(User area)
        {
            throw new NotImplementedException();
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
