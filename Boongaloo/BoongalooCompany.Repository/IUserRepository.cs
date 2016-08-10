using System;
namespace BoongalooCompany.Repository
{
    public interface IUserRepository
    {
        void AddUser(BoongalooCompany.Repository.Entities.User user);
        void AddUserClaim(string subject, string claimType, string claimValue);
        void AddUserLogin(string subject, string loginProvider, string providerKey);
        void Dispose();
        BoongalooCompany.Repository.Entities.User GetUser(string subject);
        BoongalooCompany.Repository.Entities.User GetUser(string userName, string password);
        System.Collections.Generic.IList<BoongalooCompany.Repository.Entities.UserClaim> GetUserClaims(string subject);
        BoongalooCompany.Repository.Entities.User GetUserForExternalProvider(string loginProvider, string providerKey);
        BoongalooCompany.Repository.Entities.User GetUserByEmail(string email);
        System.Collections.Generic.IList<BoongalooCompany.Repository.Entities.UserLogin> GetUserLogins(string subject);
    }
}
