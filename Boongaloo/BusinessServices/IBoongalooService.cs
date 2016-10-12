﻿using System.Collections.Generic;
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public interface IBoongalooService
    {
        #region Area specific
        AreaDto GetAreaById(int areaId);
        IEnumerable<AreaDto> GetAllAreas();
        void CreateNewArea(AreaDto area);
        IEnumerable<AreaDto> GetAreasForCoordinates(double lat, double lon);
        IEnumerable<UserDto> GetUsersFromArea(int areaId);
        #endregion

        #region Group specific
        IEnumerable<GroupDto> GetGroupsAroundCoordinates(double lat, double lon);
        long CreateNewGroup(GroupDto group);
        long CreateNewGroupAsNewArea(GroupAsNewAreaDto group);
        GroupDto GetGroupById(int id);
        IEnumerable<UserDto> GetUsersForGroup(int groupId);
        #endregion

        #region Group specific
        UserDto GetUserById(int id);
        long CreateNewUser(UserDto newUser);
        void UpdateUser(long userId, UserDto updatedEntity);
        #endregion
    }
}
