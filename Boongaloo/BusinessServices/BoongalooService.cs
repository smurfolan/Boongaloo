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
            var radiusEntity = 
                this._unitOfWork.RadiusRepository.GetRadiuses().FirstOrDefault(r => r.Id == group.RadiusId);
            // Create the new area
            var newAreaEntity = new Area()
            {
                Latitude = group.Latitude,
                Longitude = group.Longitude,
                Radius = radiusEntity
            };
            this._unitOfWork.AreaRepository.InsertArea(newAreaEntity);
            this._unitOfWork.Save();

            var areasAroundCoordinates = this.GetAreasForCoordinates(group.Latitude, group.Longitude)
                .Select(x => x.Id).ToList();

            // Fetch all the areas inside of which our coordinates fall under
            var areaEntites =
                this._unitOfWork.AreaRepository.GetAreas()
                .Where(a => areasAroundCoordinates.Contains(a.Id))
                .ToList();

            // Fetch all tag entities for this new group
            var tagEntities = this._unitOfWork.TagRepository.GetTags()
                .Where(t => group.TagIds.Contains(t.Id))
                .ToList();

            // Fetch all user entities for the newly created group. TODO: Determine if array is needed or single id
            var userEntities = this._unitOfWork.UserRepository.GetUsers()
                .Where(u => group.UserIds.Contains(u.Id))
                .ToList();

            var groupEntityToBeInserted = new Group()
            {
                Areas = areaEntites,
                Name = group.Name,
                Tags = tagEntities,
                Users = userEntities
            };

            this._unitOfWork.GroupRepository.InsertGroup(groupEntityToBeInserted);
            this._unitOfWork.Save();

            return groupEntityToBeInserted.Id;
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

        #region User specific
        public UserDto GetUserById(int id)
        {
            var userEntity = this._unitOfWork.UserRepository.GetUsers().FirstOrDefault(u => u.Id == id);

            if (userEntity == null)
                return null;

            var userAsDto = this._mapper.Map<User, UserDto>(userEntity);

            // Assign groups
            var groupsForUser = this._mapper.Map<ICollection<Group>, ICollection<GroupDto>>(userEntity.Groups);
            userAsDto.Groups = groupsForUser;

            // Assign langauges
            var languagesForUser = this._mapper.Map<ICollection<Language>, ICollection<LanguageDto>>(userEntity.Languages);
            userAsDto.Languages = languagesForUser;

            return userAsDto;
        }

        public long CreateNewUser(UserDto newUser)
        {
            if (newUser == null)
                throw new ArgumentException("The argument passed to CreateNewUser is null");

            var userAsEntity = this._mapper.Map<UserDto, User>(newUser);

            // Extract existing langugages
            if(newUser.LanguageIds != null)
            {
                var languageIdsFromDto = newUser.LanguageIds;
                var languageAsEntities = this._unitOfWork.LanguageRepository.GetLanguages().Where(l => languageIdsFromDto.Contains(l.Id));
                foreach(var language in languageAsEntities)
                {
                    userAsEntity.Languages.Add(language);
                } 
            }

            // Extract existing groups
            if(newUser.GroupIds != null)
            {
                var groupIdsFromDto = newUser.GroupIds;
                var groupsAsEntities = this._unitOfWork.GroupRepository.GetGroups().Where(g => groupIdsFromDto.Contains(g.Id));
                foreach(var group in groupsAsEntities)
                {
                    userAsEntity.Groups.Add(group);
                }
            }

            this._unitOfWork.UserRepository.InsertUser(userAsEntity);
            this._unitOfWork.Save();

            return userAsEntity.Id;
        }

        public void UpdateUser(long userId, UserDto updatedEntity)
        {
            var userToUpdate = this._unitOfWork.UserRepository.GetUsers().FirstOrDefault(u => u.Id == userId);

            this._unitOfWork.UserRepository.UpdateUser(userToUpdate);
            
            // Update languages selection TODO: Observe if old user-language realtions are removed and new ones added
            if (updatedEntity.LanguageIds != null)
            {
                userToUpdate.Languages.Clear();

                var languageIdsFromDto = updatedEntity.LanguageIds;
                var languageAsEntities = this._unitOfWork.LanguageRepository.GetLanguages().Where(l => languageIdsFromDto.Contains(l.Id));
                foreach (var language in languageAsEntities)
                {
                    userToUpdate.Languages.Add(language);
                }
            }

            // Update groups selection TODO: Observe if old user-group realtions are removed and new ones added
            if (updatedEntity.GroupIds != null)
            {
                userToUpdate.Groups.Clear();

                var groupIdsFromDto = updatedEntity.GroupIds;
                var groupsAsEntities = this._unitOfWork.GroupRepository.GetGroups().Where(g => groupIdsFromDto.Contains(g.Id));
                foreach (var group in groupsAsEntities)
                {
                    userToUpdate.Groups.Add(group);
                }
            }
            // TODO: Research if possible to use Automapper that doens't generate new entity instance
            userToUpdate.IdSrvId = updatedEntity.IdSrvId != null ? updatedEntity.IdSrvId : userToUpdate.IdSrvId;
            userToUpdate.FirstName = updatedEntity.FirstName != null ? updatedEntity.FirstName : userToUpdate.FirstName;
            userToUpdate.LastName = updatedEntity.LastName != null ? updatedEntity.LastName : userToUpdate.LastName;
            userToUpdate.Email = updatedEntity.Email != null ? updatedEntity.Email : userToUpdate.Email;
            userToUpdate.About = updatedEntity.About != null ? updatedEntity.About : userToUpdate.About;
            userToUpdate.GenderId = updatedEntity.GenderId != userToUpdate.GenderId ? updatedEntity.GenderId : userToUpdate.GenderId;
            userToUpdate.PhoneNumber = updatedEntity.PhoneNumber != null ? updatedEntity.PhoneNumber : userToUpdate.PhoneNumber;
            userToUpdate.BirthDate = updatedEntity.BirthDate != null ? updatedEntity.BirthDate : userToUpdate.BirthDate;

            this._unitOfWork.Save();
        }
        #endregion
    }
}
