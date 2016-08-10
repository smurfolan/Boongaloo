using System;
using System.Linq;
using Boongaloo.DTO.Applicant;

namespace Boongaloo.Repository
{
    public class UserDataRepository : IUserDataRepository, IDisposable
    {
        UserDataContext _ctx;

        public UserDataRepository(UserDataContext userContext)
        {
            _ctx = userContext;
        }

        public UserDataRepository()
        {
            // no context passed in, assume default location
            _ctx = new UserDataContext(@"app_data/userinfostore.json");
        }

        public void AddUser(UserData userData)
        {
            _ctx.Users.Add(userData);

            this.Save();
        }

        public UserData GetUser(string subject)
        {
            // We expect identity_provider + issuer for the subject. This is what uniquely identifies a user.
            return _ctx.Users.FirstOrDefault(u => u.UniqueId.ToLowerInvariant() == subject.ToLowerInvariant());
        }

        public void UpdateUser(UserData user)
        {
            var userToBeUpdated = _ctx.Users.FirstOrDefault(userData => userData.UniqueId == user.UniqueId);

            if (userToBeUpdated != null)
                userToBeUpdated = user;//Need to pay attention to that. Most likely copy is not made. Make copy constructor or AutoMap them.

            this.Save();
        }

        public void DeleteUser(string userId)
        {
            var userToBeDeleted = _ctx.Users.FirstOrDefault(x => x.UniqueId == userId);
            _ctx.Users.Remove(userToBeDeleted);

            this.Save();
        }

        public void Dispose()
        {
            // Do a cleanup
            // throw new NotImplementedException();
        }

        private bool Save()
        {
            return _ctx.SaveChanges();
        }
    }
}
