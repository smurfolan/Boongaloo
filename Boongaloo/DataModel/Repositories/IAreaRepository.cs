using System.Collections.Generic;

namespace DataModel.Repositories
{
    public interface IAreaRepository
    {
        IEnumerable<Area> GetAreas();
        Area GetAreaById(int areaId);
        IEnumerable<Area> GetAreasForGroupId(int groupId);

        void InsertArea(Area area);
        void DeleteArea(int areaId);
        void UpdateArea(Area area);

        IEnumerable<Area> GetAreas(double latitude, double longitude);

        void Save();
    }
}
