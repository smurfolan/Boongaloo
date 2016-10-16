using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Boongaloo.DTO.BoongalooWebApiDto;
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

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }
        public User GetUserById(int userId)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == userId);
        }
        public IEnumerable<User> GetUsersForGroupId(int groupId)
        {
            return (from item1 in _dbContext.GroupToUser
                    join item2 in _dbContext.Users
                    on item1.UserId equals item2.Id
                    where item1.GroupId == groupId
                    select item2).ToList();
        }

        public void InsertUser(User user)
        {
            user.Id = this.GetUsers().Count() + 1;
            _dbContext.Users.Add(user);
        }
        public void DeleteUser(int userId)
        {
            var userToBeDeleted = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            _dbContext.Users.Remove(userToBeDeleted);
        }
        public void UpdateUser(User user)
        {
            var userToBeUpdated = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToBeUpdated == null)
                return;

            userToBeUpdated.About = user.About;
            userToBeUpdated.BirthDate = user.BirthDate;
            userToBeUpdated.Email = user.Email;
            userToBeUpdated.FirstName = user.FirstName;
            //userToBeUpdated.Langugages = user.Langugages;
            userToBeUpdated.LastName = user.LastName;
            userToBeUpdated.Gender = user.Gender;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;
        }

        public void UpdateUserSubscriptionsToGroups(int userId, IEnumerable<GroupSubscriptionDto> groupSubscriptions)
        {
            foreach (var groupSubscriptionDto in groupSubscriptions)
            {
                if (groupSubscriptionDto.IsSubscribtionRequest)
                {
                    var lastRecord = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextRecordId = lastRecord?.Id + 1 ?? 1;

                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        GroupId = groupSubscriptionDto.GroupId,
                        UserId = userId,
                        Id = nextRecordId
                    });
                }
                else
                {
                    var recordToBeRemoved = _dbContext.GroupToUser
                        .FirstOrDefault(x => x.GroupId == groupSubscriptionDto.GroupId && x.UserId == userId);
                    this._dbContext.GroupToUser.Remove(recordToBeRemoved);
                }
            }
        }


        public IEnumerable<UserResponseDto> GetUsersFromGroup(int groupId)
        {
            var result = new List<UserResponseDto>();

            var userIds = this._dbContext.GroupToUser.Where(x => x.GroupId == groupId).Select(y => y.UserId);
            var userEntities = this._dbContext.Users.Where(u => userIds.Contains(u.Id));

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
    }
}
