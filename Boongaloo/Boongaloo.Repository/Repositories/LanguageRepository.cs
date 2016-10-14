using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using Boongaloo.Repository.Entities;

namespace Boongaloo.Repository.Repositories
{
    public class LanguageRepository : ILanguageRepository, IDisposable
    {
        private readonly BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public LanguageRepository(BoongalooDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Language> GetLangauges()
        {
            return this._dbContext.Languages;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
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
