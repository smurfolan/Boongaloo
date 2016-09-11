using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        void Save();
    }
}
