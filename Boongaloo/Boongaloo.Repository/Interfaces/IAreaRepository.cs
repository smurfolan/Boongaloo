using System.Collections.Generic;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Interfaces
{
    public interface IAreaRepository
    {
        IEnumerable<Area> GetAreas();
        Area GetAreaById(int areaId);
        IEnumerable<Area> GetAreasForGroupId(int groupId); 

        void InsertArea(Area area);
        void DeleteArea(int areaId);
        void UpdateArea(Area area);

        void Save();
    }
}
