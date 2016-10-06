using System.Collections.Generic;
using BusinessEntities;
using DataModel;

namespace BusinessServices
{
    public interface IBoongalooService
    {
        AreaDto GetAreaById(int areaId);

        IEnumerable<AreaDto> GetAllAreas();

        void CreateNewArea(AreaDto area);
    }
}
