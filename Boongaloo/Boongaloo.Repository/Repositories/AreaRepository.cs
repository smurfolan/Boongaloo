using System;
using System.Collections.Generic;
using System.Device.Location;/*Consider moving this away and replacing it with own calculations class.*/
using System.Linq;
using AutoMapper;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;
using Boongaloo.DTO.Enums;
using Boongaloo.Repository.Automapper;
using Boongaloo.Repository.BoongalooDtos;

namespace Boongaloo.Repository.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;
        private readonly IMapper _mapper;

        public AreaRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public IEnumerable<Area> GetAreas()
        {
            return _dbContext.Areas.ToList();
        }
        public AreaResponseDto GetAreaById(int areaId)
        {
            var areaEntity = _dbContext.Areas.FirstOrDefault(x => x.Id == areaId);

            if (areaEntity == null)
                return null;

            var areaDto = this._mapper.Map<Area, AreaResponseDto>(areaEntity);

            var groupIds = this._dbContext.AreaToGroup
                .Where(ul => ul.AreaId == areaEntity.Id)
                .Select(l => l.GroupId);

            var groupDtos = new List<GroupResponseDto>();

            foreach (var groupId in groupIds)
            {
                var groupEntity = this._dbContext.Groups.FirstOrDefault(g => g.Id == groupId);
                var result = new GroupResponseDto();
                this.MapToGroupResponseDto(groupEntity, result);
                groupDtos.Add(result);
            }

            areaDto.Groups = groupDtos;

            return areaDto;
        }

        public IEnumerable<Area> GetAreasForGroupId(int groupId)
        {
            return (from item1 in _dbContext.AreaToGroup
                    join item2 in _dbContext.Areas
                    on item1.AreaId equals item2.Id
                    where item1.GroupId == groupId
                    select item2).ToList();
        }

        public int InsertArea(Area area)
        {
            var areas = this.GetAreas();
            area.Id = areas.Count() > 0 ? areas.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1 : 1;
            _dbContext.Areas.Add(area);

            return area.Id;
        }
        public void DeleteArea(int areaId)
        {
            var toBeDeleted = _dbContext.Areas.FirstOrDefault(x => x.Id == areaId);
            _dbContext.Areas.Remove(toBeDeleted);
        }
        public void UpdateArea(Area area)
        {
            var toBeUpdated = _dbContext.Areas.FirstOrDefault(x => x.Id == area.Id);

            if (toBeUpdated == null)
                return;

            toBeUpdated.Latitude = area.Latitude;
            toBeUpdated.Longitude = area.Longitude;
            toBeUpdated.Radius = area.Radius;
        }

        public IEnumerable<Area> GetAreas(double latitude, double longitude)
        {
            var currentUserLocation = new GeoCoordinate(latitude, longitude);

            return this._dbContext.Areas
                .Where(x => currentUserLocation.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)) <= (int)x.Radius);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
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

        private void MapToGroupResponseDto(Group groupEntity, GroupResponseDto result)
        {
            result.Name = groupEntity.Name;
            result.Id = groupEntity.Id;

            // Add all the tags to result
            var tagsIdsForTheGroup = this._dbContext.GroupToTag
                .Where(x => x.GroupId == groupEntity.Id)
                .Select(y => y.TagId);

            var tagEntities = this._dbContext.Tags.Where(t => tagsIdsForTheGroup.Contains(t.Id));
            var tagDtos = this._mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tagEntities);

            result.Tags = tagDtos;

            // Add all the users to the result
            var userIdsForTheGroup = this._dbContext.GroupToUser
                .Where(x => x.GroupId == groupEntity.Id)
                .Select(y => y.UserId);

            var userEntities = this._dbContext.Users.Where(t => userIdsForTheGroup.Contains(t.Id));
            var userDtos = this._mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(userEntities);

            result.Users = userDtos;
        }
    }
}
