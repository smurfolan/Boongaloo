using Boongaloo.DTO.Applicant;

namespace Boongaloo.Repository
{
    public interface IUserDataRepository
    {
        void AddUser(UserData user);
        UserData GetUser(string subject);
        void UpdateUser(UserData user);
        void DeleteUser(string subject);
        void Dispose();
    }
}
