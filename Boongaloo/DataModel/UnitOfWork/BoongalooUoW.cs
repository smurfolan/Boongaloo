using System;

namespace DataModel.UnitOfWork
{
    public class BoongalooUoW : IDisposable
    {
        private readonly BoongalooDbCtx _dbContext = new BoongalooDbCtx();

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
