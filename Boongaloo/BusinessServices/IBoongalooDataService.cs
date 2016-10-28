using BusinessEntities;
using System.Collections.Generic;

namespace BusinessServices
{
    public interface IBoongalooDataService
    {
        IEnumerable<MyEntityDto> GetAllMyEntities();
    }
}
