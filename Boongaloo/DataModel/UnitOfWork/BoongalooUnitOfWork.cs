using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Repositories;

namespace DataModel.UnitOfWork
{
    public class BoongalooUnitOfWork
    {
        private readonly BoongalooDbContext _dbContext;

        public BoongalooUnitOfWork()
        {
            using (var db = new BoongalooDbContext())
            {
                db.Database.EnsureCreated();

                this._dbContext = new BoongalooDbContext();
            }
        }

        private MyEntityRepository myEntityRepository;

        public MyEntityRepository MyEntityRepository
        {
            get
            {
                if (myEntityRepository == null)
                    return new MyEntityRepository(_dbContext);

                return myEntityRepository;
            }
        }

        private bool _disposed = false;

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
