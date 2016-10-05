using System.Collections.Generic;
using BusinessEntities;

namespace BusinessServices
{
    public interface IBoongalooService
    {
        AreaDto GetAreaById(int areaId);

        IEnumerable<AreaDto> GetAllAreas();
    }
}
