using Boongaloo.Repository.Interfaces;
using System;
using System.Collections.Generic;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Contexts;

namespace Boongaloo.Repository.Repositories
{
    public class TagRepository : ITagRepository, IDisposable
    {
        private readonly BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public TagRepository(BoongalooDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Tag> GetTags()
        {
            return this._dbContext.Tags;
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
