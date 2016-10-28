using System;
using System.Collections.Generic;

namespace DataModel.Repositories
{
    public class MyEntityRepository : IMyEntityRepository
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public MyEntityRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<MyEntity> GetMyEntities()
        {
            return this._dbContext.MyEntities;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }   
}
