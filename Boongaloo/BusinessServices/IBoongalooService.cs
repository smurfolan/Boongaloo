using System.Collections.Generic;
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
    }
}
