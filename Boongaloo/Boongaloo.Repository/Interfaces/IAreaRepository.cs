using System.Collections.Generic;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    public interface IAreaRepository
    {
        IEnumerable<Area> GetAreas();
        AreaResponseDto GetAreaById(int areaId);
        IEnumerable<Area> GetAreasForGroupId(int groupId); 

        int InsertArea(Area area);
        void DeleteArea(int areaId);
        void UpdateArea(Area area);

        IEnumerable<Area> GetAreas(double latitude, double longitude);

        void Save();
    }
}
