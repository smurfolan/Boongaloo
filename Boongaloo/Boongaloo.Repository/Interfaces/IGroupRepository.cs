using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetGroups();

        Group GetGroupById(int groupId);

        void InsertGroup(Group groupToInsert);

        void DeleteGroup(int groupId);

        void UpdateGroup(Group updatedGroup);

        void Save();
    }
}
