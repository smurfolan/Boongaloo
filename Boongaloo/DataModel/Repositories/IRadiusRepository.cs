using System.Collections.Generic;

namespace DataModel.Repositories
{
    public interface IRadiusRepository
    {
        IEnumerable<Radius> GetRadiuses();
        void Save();
    }
}
