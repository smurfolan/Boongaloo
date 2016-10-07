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

        public void CreateNewGroup(GroupDto group)
        {
            if (group == null)
                throw new ArgumentException("The argument passed to CreateNewGroup is null");

            var groupAsEntity = this._mapper.Map<GroupDto, Group>(group);

            this._unitOfWork.GroupRepository.InsertGroup(groupAsEntity);

            this._unitOfWork.Save();
        }

        public GroupDto GetGroupById(int id)
        {
            var groupEntity = this._unitOfWork.GroupRepository.GetGroups().FirstOrDefault(g => g.Id == id);

            if (groupEntity == null)
                return null;

            var groupAsDto = this._mapper.Map<Group, GroupDto>(groupEntity);

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
