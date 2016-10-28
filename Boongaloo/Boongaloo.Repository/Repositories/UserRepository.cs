using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Boongaloo.Repository.Automapper;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;
using GroupDto = Boongaloo.Repository.BoongalooDtos.GroupDto;

namespace Boongaloo.Repository.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;
        private readonly IMapper _mapper;

        public UserRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IEnumerable<UserResponseDto> GetUsers()
        {
            var result = new List<UserResponseDto>();

            var userEntities = _dbContext.Users.ToList();

            this.MapUsersToUserDtos(userEntities, result);

            return result;
        }

        public UserResponseDto GetUserById(int userId)
        {
            var userEntity = _dbContext.Users.FirstOrDefault(x => x.Id == userId);

            if (userEntity == null)
                return null;

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

            return userDto;
        }
        public IEnumerable<User> GetUsersForGroupId(int groupId)
        {
            return (from item1 in _dbContext.GroupToUser
                    join item2 in _dbContext.Users
                    on item1.UserId equals item2.Id
                    where item1.GroupId == groupId
                    select item2).ToList();
        }

        public UserResponseDto GetUserByStsId(string stsId)
        {
            var userEntity = this._dbContext.Users.FirstOrDefault(u => u.IdsrvUniqueId == stsId);

            if (userEntity == null)
                return null;

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

            return userDto;
        }

        public int InsertUser(NewUserRequestDto user)
        {
            var latestUserRecord = this.GetUsers().OrderBy(x => x.Id).LastOrDefault();
            var nextUserId = latestUserRecord != null ? latestUserRecord.Id + 1 : 1;

            // Create user entity and add it to users
            var userEntity = this._mapper.Map<NewUserRequestDto, User>(user);
            userEntity.Id = nextUserId;
            this._dbContext.Users.Add(userEntity);

            if (user.LanguageIds != null && user.LanguageIds.Any())
            {
                // Foreach language add new pair in UserToLanguage store
                foreach (var languageId in user.LanguageIds)
                {
                    var nextRecordId = this._dbContext.UserToLangauge.OrderBy(x => x.Id).LastOrDefault();
                    var nextId = nextRecordId?.Id + 1 ?? 1;
                    this._dbContext.UserToLangauge.Add(new UserToLanguage()
                    {
                        Id = nextId,
                        LanguageId = languageId,
                        UserId = nextUserId
                    });
                }
            }

            if (user.GroupIds != null && user.GroupIds.Any())
            {
                // Foreach group add new pair in the UserToGroup store
                foreach (var groupId in user.GroupIds)
                {
                    var nextRecordId = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextId = nextRecordId?.Id + 1 ?? 1;
                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        Id = nextId,
                        GroupId = groupId,
                        UserId = nextUserId
                    });
                }
            }

            // Create default user notification Settings
            var latestNotificationSettingsRecord = _dbContext.UserNotificationSettings.OrderBy(x => x.Id).LastOrDefault();
            var nextNotificationSettingsId = latestNotificationSettingsRecord != null ? latestNotificationSettingsRecord.Id + 1 : 1;

            this._dbContext.UserNotificationSettings.Add(new UserNotificationSettings
            {
                Id = nextNotificationSettingsId,
                AutomaticallySubscribeToAllGroups = true,
                AutomaticallySubscribeToAllGroupsWithTag = false,
                UserId = nextUserId,
                SubscribedTagIds = new List<int>()
            });

            this._dbContext.SaveChanges();

            return nextUserId;
        }
        public void DeleteUser(int userId)
        {
            var userToBeDeleted = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            _dbContext.Users.Remove(userToBeDeleted);
        }
        public void UpdateUser(NewUserRequestDto user)
        {
            var userToBeUpdated = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToBeUpdated == null)
                return;

            userToBeUpdated.About = user.About;
            userToBeUpdated.BirthDate = user.BirthDate;
            userToBeUpdated.Email = user.Email;
            userToBeUpdated.FirstName = user.FirstName;
            userToBeUpdated.LastName = user.LastName;
            userToBeUpdated.Gender = user.Gender;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;

            // Remove old language records
            var languageRecordsToRemove = this._dbContext.UserToLangauge
                .Where(utl => utl.UserId == user.Id)
                .ToList();

            for(int i = 0 ; i < languageRecordsToRemove.Count(); i++)
                this._dbContext.UserToLangauge.Remove(languageRecordsToRemove[i]);

            // Add new language records
            if (user.LanguageIds != null && user.LanguageIds.Any())
            {
                foreach (var languageId in user.LanguageIds)
                {
                    var nextRecordId = this._dbContext.UserToLangauge.OrderBy(x => x.Id).LastOrDefault();
                    var nextId = nextRecordId?.Id + 1 ?? 1;
                    this._dbContext.UserToLangauge.Add(new UserToLanguage()
                    {
                        Id = nextId,
                        LanguageId = languageId,
                        UserId = user.Id
                    });
                }
            }         

            // Remove old group records
            var groupRecordsToRemove = this._dbContext.GroupToUser
                .Where(gtu => gtu.UserId == user.Id)
                .ToList();

            for (int i = 0; i < groupRecordsToRemove.Count(); i++)
                this._dbContext.GroupToUser.Remove(groupRecordsToRemove[i]);

            // Add new group records
            if (user.GroupIds != null && user.GroupIds.Any())
            {
                foreach (var groupId in user.GroupIds)
                {
                    var nextRecordId = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextId = nextRecordId?.Id + 1 ?? 1;
                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        Id = nextId,
                        GroupId = groupId,
                        UserId = user.Id
                    });
                }
            }          

            this._dbContext.SaveChanges();
        }

        public void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<int> groupSubscriptions)
        {
            // 1. Remove all the records from GroupUser If groupSubscriptions doesn't contain <userId - groupId>
            var relationsToBeRemoved = this._dbContext.GroupToUser
                .Where(pair => pair.UserId == userId && !groupSubscriptions.Contains(pair.GroupId))
                .ToList();

            for(int i = 0; i < relationsToBeRemoved.Count() ; i++)
            {
                this._dbContext.GroupToUser.Remove(relationsToBeRemoved[i]);
            }
            
            // 2. Foreach pair <userId - groupId> - if available in GroupUsers - continue, otherwise add new record.
            foreach(var subscription in groupSubscriptions)
            {
                if(this._dbContext.GroupToUser.FirstOrDefault(pair => pair.UserId == userId && pair.GroupId == subscription) != null)
                {
                    continue;
                }
                else
                {
                    var nextRecordId = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextId = nextRecordId?.Id + 1 ?? 1;
                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        Id = nextId,
                        GroupId = subscription,
                        UserId = userId
                    });
                }
            }
        }

        public IEnumerable<UserResponseDto> GetUsersFromGroup(int groupId)
        {
            var result = new List<UserResponseDto>();

            var userIds = this._dbContext.GroupToUser.Where(x => x.GroupId == groupId).Select(y => y.UserId);
            var userEntities = this._dbContext.Users.Where(u => userIds.Contains(u.Id)).ToList();

            this.MapUsersToUserDtos(userEntities, result);

            return result;
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
        public IEnumerable<int> GetUserSubscriptions(int uid)
        {
            var result = new List<int>();

            var groupIds = this._dbContext.GroupToUser
                .Where(gtu => gtu.UserId == uid)
                .Select(r => r.GroupId)
                .Distinct()
                .ToList();

            return groupIds;
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
