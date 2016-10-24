using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;

namespace DataModel.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private BoongalooDbCtx _dbContext;

        private bool _disposed = false;

        public AreaRepository(BoongalooDbCtx dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Area> GetAreas()
        {
            return this._dbContext.Areas;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
