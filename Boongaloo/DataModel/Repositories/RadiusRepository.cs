using System;
using System.Collections.Generic;

namespace DataModel.Repositories
{
    public class RadiusRepository : IRadiusRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public RadiusRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Radius> GetRadiuses()
        {
            return _dbContext.Radiuses;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
