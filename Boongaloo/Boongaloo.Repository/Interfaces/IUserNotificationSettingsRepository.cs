using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;
using System.Collections.Generic;

namespace Boongaloo.Repository.Interfaces
{
    public interface IUserNotificationSettingsRepository
    {
        IEnumerable<UserNotificationSettings> GetAllUserNotificationSettings();
        UserNotificationSettingsResponseDto GetNotificationSettingsForUserWithId(int userId);
        int InsertNewNotificationSettings(EditUserNotificationsRequestDto userNotificationSettings);
        void UpdateUserNotificationSettings(int id, EditUserNotificationsRequestDto edittedInfo);
    }
}
