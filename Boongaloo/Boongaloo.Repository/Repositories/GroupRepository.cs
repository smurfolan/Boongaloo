using System;
using System.Collections.Generic;
using System.Device.Location;/*Consider moving this away and replacing it with own calculations class.*/
using System.Linq;
using AutoMapper;
using Boongaloo.Repository.Automapper;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;

namespace Boongaloo.Repository.Repositories
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        private readonly BoongalooDbContext _dbContext;

        private bool _disposed = false;
        private readonly IMapper _mapper;

        public GroupRepository(BoongalooDbContext dbContext)
        {
            _dbContext = dbContext;
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IEnumerable<Group> GetGroups()
        {
            return this._dbContext.Groups;
        }
        public GroupResponseDto GetGroupById(int groupId)
        {
            var result = new GroupResponseDto();

            var groupEntity = this._dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
            if (groupEntity == null)
                return null;

            result.Id = groupEntity.Id;
            result.Name = groupEntity.Name;

            this.MapToGroupResponseDto(groupEntity, result);

            return result;
        }

        private void MapToGroupResponseDto(Group groupEntity, GroupResponseDto result)
        {
            // Add all the tags to result
            var tagsIdsForTheGroup = this._dbContext.GroupToTag
                .Where(x => x.GroupId == groupEntity.Id)
                .Select(y => y.TagId);

            var tagEntities = this._dbContext.Tags.Where(t => tagsIdsForTheGroup.Contains(t.Id));
            var tagDtos = this._mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tagEntities);

            result.Tags = tagDtos;
            // Add all the areas to the result
            var areaIdsForTheGroup = this._dbContext.AreaToGroup
                .Where(x => x.GroupId == groupEntity.Id)
                .Select(y => y.AreaId);

            var areaEntities = this._dbContext.Areas.Where(t => areaIdsForTheGroup.Contains(t.Id));
            var areaDtos = this._mapper.Map<IEnumerable<Area>, IEnumerable<AreaDto>>(areaEntities);

            result.Areas = areaDtos;

            // Add all the users to the result
            var userIdsForTheGroup = this._dbContext.GroupToUser
                .Where(x => x.GroupId == groupEntity.Id)
                .Select(y => y.UserId);

            var userEntities = this._dbContext.Users.Where(t => userIdsForTheGroup.Contains(t.Id));
            var userDtos = this._mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(userEntities);

            result.Users = userDtos;
        }

        public IEnumerable<Group> GetGroupsForUserId(int userId)
        {
            return (from item1 in _dbContext.GroupToUser
                    join item2 in _dbContext.Groups
                    on item1.GroupId equals item2.Id
                    where item1.UserId == userId
                    select item2).ToList();
        }
        public IEnumerable<Group> GetGroupsForAreaId(int areaId)
        {
            return (from item1 in _dbContext.AreaToGroup
                    join item2 in _dbContext.Groups
                    on item1.GroupId equals item2.Id
                    where item1.AreaId == areaId
                    select item2).ToList();
        }

        public int InsertGroup(Group groupToInsert)
        {
            var latestGroupRecord = this.GetGroups().OrderBy(x => x.Id).LastOrDefault();
            var nextGroupId = latestGroupRecord?.Id + 1 ?? 1;

            groupToInsert.Id = nextGroupId;

            this._dbContext.Groups.Insert(0, groupToInsert);

            return nextGroupId;
        }
        public void DeleteGroup(int groupId)
        {
            var groupToBeDeleted = this._dbContext.Groups.FirstOrDefault(x => x.Id == groupId);
            this._dbContext.Groups.Remove(groupToBeDeleted);
        }
        public void UpdateGroup(Group updatedGroup)
        {
            var toBeUpdated = this._dbContext.Groups.FirstOrDefault(x => x.Id == updatedGroup.Id);

            if (toBeUpdated == null)
                return;

            toBeUpdated.Name = updatedGroup.Name;
            //toBeUpdated.Tags = updatedGroup.Tags;
        }

        // Consider moving these away when DbContext is changes. We keep these methods because we are artificially maintaining RDB
        // using JSON files. We manage our ow bridge tables and maintain them by our ow which is not a good practice.
        public int InsertGroup(
            Group grouToInsert, 
            IEnumerable<int> areaIds, 
            IEnumerable<int> tagIds, 
            IEnumerable<int> userIds)
        {
            var latestGroupRecord = this.GetGroups().OrderBy(x => x.Id).LastOrDefault();
            var nextGroupId = latestGroupRecord?.Id + 1 ?? 1;

            grouToInsert.Id = nextGroupId;
            
            // Insert into the bridge table AreaGroup
            var allAreaIds = _dbContext.Areas.Select(a => a.Id).ToList();
            if(areaIds != null)
            {
                foreach (var areaId in areaIds)
                {
                    if (!allAreaIds.Contains(areaId))
                        continue;

                    var latestAreaToGroupRecord = this._dbContext.AreaToGroup.OrderBy(x => x.Id).LastOrDefault();
                    var nextAreaToGroupId = latestAreaToGroupRecord?.Id + 1 ?? 1;

                    this._dbContext.AreaToGroup.Add(new AreaToGroup()
                    {
                        AreaId = areaId,
                        GroupId = grouToInsert.Id,
                        Id = nextAreaToGroupId
                    });
                }
            }           

            // Insert into the bridge table UserGroup
            var allUserIds = _dbContext.Users.Select(u => u.Id).ToList();
            if(userIds != null)
            {
                foreach (var userId in userIds)
                {
                    if (!allUserIds.Contains(userId))
                        continue;

                    var latestAreaToGroupRecord = this._dbContext.GroupToUser.OrderBy(x => x.Id).LastOrDefault();
                    var nextUserToGroupId = latestAreaToGroupRecord?.Id + 1 ?? 1;

                    this._dbContext.GroupToUser.Add(new GroupToUser()
                    {
                        UserId = userId,
                        GroupId = grouToInsert.Id,
                        Id = nextUserToGroupId
                    });
                }
            }
            

            // Insert into the bridge table GroupTags
            var allTagIds = _dbContext.Tags.Select(t => t.Id).ToList();
            if(tagIds != null)
            {
                foreach (var tagId in tagIds)
                {
                    if (!allTagIds.Contains(tagId))
                        continue;

                    var latestTagToGroupRecord = this._dbContext.GroupToTag.OrderBy(x => x.Id).LastOrDefault();
                    var nextTagToGroupId = latestTagToGroupRecord?.Id + 1 ?? 1;

                    this._dbContext.GroupToTag.Add(new GroupToTag()
                    {
                        TagId = tagId,
                        GroupId = grouToInsert.Id,
                        Id = nextTagToGroupId
                    });
                }
            }     

            // Insert the new group
            this._dbContext.Groups.Add(new Group()
            {
                Id = grouToInsert.Id,
                Name = grouToInsert.Name
            });

            return grouToInsert.Id;
        }

        public IEnumerable<GroupResponseDto> GetGroups(double latitude, double longitude)
        {
            var currentUserLocation = new GeoCoordinate(latitude, longitude);

            var areasInsideOfWhichUserIsCurrentlyIn = this._dbContext.Areas
                .Where(x => currentUserLocation.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)) <= (int)x.Radius)
                .Select(y => y.Id);

            var groupIds =
                this._dbContext.AreaToGroup.Where(x => areasInsideOfWhichUserIsCurrentlyIn.Contains(x.AreaId))
                    .Select(m => m.GroupId);

            var result = new List<GroupResponseDto>();

            foreach(var groupId in groupIds)
            {
                var groupEntity = this._dbContext.Groups.FirstOrDefault(gr => gr.Id == groupId);
                var newGroupDto = new GroupResponseDto();

                newGroupDto.Id = groupEntity.Id;
                newGroupDto.Name = groupEntity.Name;

                this.MapToGroupResponseDto(groupEntity, newGroupDto);

                result.Add(newGroupDto);
            }         

            return result;
        }

        public void Save()
        {
            this._dbContext.SaveChanges();
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
