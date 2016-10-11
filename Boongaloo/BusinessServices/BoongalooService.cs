using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class BoongalooService : IBoongalooService
    {
        private readonly BoongalooUoW _unitOfWork;

        private readonly IMapper _mapper;

        public BoongalooService()
        {
            _unitOfWork = new BoongalooUoW();
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        #region Area specific
        public AreaDto GetAreaById(int areaId)
        {
            var area = _unitOfWork.AreaRepository.GetAreaById(areaId);

            if (area == null)
                return null;

            var areaAsDto = this._mapper.Map<Area, AreaDto>(area);

            return areaAsDto;
        }

        public IEnumerable<AreaDto> GetAllAreas()
        {
            throw new NotImplementedException();
        }

        public void CreateNewArea(AreaDto area)
        {
            if (area == null)
                throw new ArgumentException("The argument passed to CreateNewArea is null");

            var areaAsEntity = this._mapper.Map<AreaDto, Area>(area);

            this._unitOfWork.AreaRepository.InsertArea(areaAsEntity);

            this._unitOfWork.Save();
        }

        public IEnumerable<AreaDto> GetAreasForCoordinates(double lat, double lon)
        {
            var currentUserLocation = new GeoCoordinate(lat, lon);

            var areaEntities = this._unitOfWork.AreaRepository.GetAreas().ToList()
                .Where(x => currentUserLocation.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)) <= x.Radius.Range);

            var result = this._mapper.Map<IEnumerable<Area>, IEnumerable<AreaDto>>(areaEntities);

            return result;
        }

        public IEnumerable<UserDto> GetUsersFromArea(int areaId)
        {
            var ourArea = _unitOfWork.AreaRepository
                .GetAreas()
                .FirstOrDefault(x => x.Id == areaId);

            if (ourArea == null)
                return null;

            var usersResult = new List<UserDto>();

            foreach (var areaGroup in ourArea.Groups)
            {
                var usersAsDtos = this._mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(areaGroup.Users.ToList());
                usersResult.AddRange(usersAsDtos);
            }

            return usersResult;
        }

        #endregion

        #region Group specific
        public IEnumerable<GroupDto> GetGroupsAroundCoordinates(double lat, double lon)
        {
            var areasAroundCoordinates = this.GetAreasForCoordinates(lat, lon);

            var result = new List<GroupDto>();

            foreach (var area in areasAroundCoordinates)
            {
                result.AddRange(area.Groups);
            }

            return result;
        }

        public long CreateNewGroup(GroupDto group)/*Name, TagIds, AreaIds*/
        {
            if (group == null)
                throw new ArgumentException("The argument passed to CreateNewGroup is null");

            var groupAsEntity = this._mapper.Map<GroupDto, Group>(group);

            // Extracting existing areas
            if(group.AreaIds != null)
            {
                var areaIdsFromDto = group.AreaIds;
                var areaEntities = this._unitOfWork.AreaRepository.GetAreas().Where(a => areaIdsFromDto.Contains(a.Id));
                foreach (var areaAsEntity in areaEntities)
                {
                    groupAsEntity.Areas.Add(areaAsEntity);
                }
            }

            // Extracting existing tags
            var tagsFromDto = group.TagIds;
            var tagAsEntities = this._unitOfWork.TagRepository.GetTags().Where(t => tagsFromDto.Contains(t.Id));
            foreach(var tagEntity in tagAsEntities)
            {
                groupAsEntity.Tags.Add(tagEntity);
            }

            // Extracting existing users
            if(group.UserIds != null)
            {
                var usersFromDto = group.UserIds;
                var usersAsEntities = this._unitOfWork.UserRepository.GetUsers().Where(u => usersFromDto.Contains(u.Id));
                foreach (var userEntity in usersAsEntities)
                {
                    groupAsEntity.Users.Add(userEntity);
                }
            }

            this._unitOfWork.GroupRepository.InsertGroup(groupAsEntity);

            this._unitOfWork.Save();

            return groupAsEntity.Id;
        }

        public long CreateNewGroupAsNewArea(GroupAsNewAreaDto group)/*latitude, longitude, radiusId*/
        {
            throw new NotImplementedException();
        }

        public GroupDto GetGroupById(int id)
        {
            var groupEntity = this._unitOfWork.GroupRepository.GetGroups().FirstOrDefault(g => g.Id == id);

            if (groupEntity == null)
                return null;

            // Assing tags
            var groupAsDto = this._mapper.Map<Group, GroupDto>(groupEntity);
            var tagsForGroup = this._mapper.Map<ICollection<Tag>, ICollection<TagDto>>(groupEntity.Tags);
            groupAsDto.Tags = tagsForGroup;

            // Assign areas
            var areasForGroup = this._mapper.Map<ICollection<Area>, ICollection<AreaDto>>(groupEntity.Areas);
            groupAsDto.Areas = areasForGroup;

            // Assign users
            var usersForGroup = this._mapper.Map<ICollection<User>, ICollection<UserDto>>(groupEntity.Users);
            groupAsDto.Users = usersForGroup;

            return groupAsDto;
        }

        public IEnumerable<UserDto> GetUsersForGroup(int groupId)
        {
            var selectedGroup = this._unitOfWork.GroupRepository.GetGroups().FirstOrDefault(g => g.Id == groupId);

            if (selectedGroup == null)
                return null;

            var groupUsers = this._mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(selectedGroup.Users);

            return groupUsers;
        }

        #endregion
    }
}
