using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Repositories
{
    public interface IAreaRepository
    {
        IEnumerable<Area> GetAreas();
    }
}
