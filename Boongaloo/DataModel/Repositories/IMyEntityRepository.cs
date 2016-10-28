using System.Collections.Generic;

namespace DataModel.Repositories
{
    public interface IMyEntityRepository
    {
        IEnumerable<MyEntity> GetMyEntities();
    }
}
