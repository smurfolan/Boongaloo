using System;
using System.Collections.Generic;

namespace DataModel.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public LanguageRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Language> GetLanguages()
        {
            return this._dbContext.Languages;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
