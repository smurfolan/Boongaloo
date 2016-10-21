using AutoMapper;
using Boongaloo.Repository.Automapper;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Boongaloo.Repository.Repositories
{
    public class UserNotificationSettingsRepository : IUserNotificationSettingsRepository
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;
        private readonly IMapper _mapper;

        public UserNotificationSettingsRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public int InsertNewNotificationSettings(EditUserNotificationsRequestDto userNotificationSettings)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserNotificationSettings> GetAllUserNotificationSettings()
        {
            return this._dbContext.UserNotificationSettings;
        }

        public UserNotificationSettingsResponseDto GetNotificationSettingsForUserWithId(int userId)
        {
            var notificationSettingsEntity = this._dbContext.UserNotificationSettings.FirstOrDefault(us => us.UserId == userId);

            var notificationSettingsDto = this._mapper.Map<UserNotificationSettings, UserNotificationSettingsResponseDto>(notificationSettingsEntity);

            return notificationSettingsDto;
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

        private void MapUsersToUserDtos(List<User> userEntities, List<UserResponseDto> result)
        {
            foreach (var userEntity in userEntities)
            {
                var userDto = this._mapper.Map<User, UserResponseDto>(userEntity);

                // 1. Add languages
                var langaugeIds = this._dbContext.UserToLangauge
                    .Where(ul => ul.UserId == userEntity.Id)
                    .Select(l => l.LanguageId);
                var languageEntities = this._dbContext.Languages.Where(l => langaugeIds.Contains(l.Id));
                var languageDtos = this._mapper.Map<IEnumerable<Language>, IEnumerable<LanguageDto>>(languageEntities);

                userDto.Langugages = languageDtos;
                // 2. Add groups
                var groupIds = this._dbContext.GroupToUser
                    .Where(ul => ul.UserId == userEntity.Id)
                    .Select(l => l.GroupId);
                var groupEntities = this._dbContext.Groups.Where(gr => groupIds.Contains(gr.Id));
                var groupDtos = this._mapper.Map<IEnumerable<Group>, IEnumerable<GroupDto>>(groupEntities);

                userDto.Groups = groupDtos;
                result.Add(userDto);
            }
        }
    }
}
