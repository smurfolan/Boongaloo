using System;
using System.Collections.Generic;

namespace DataModel.Repositories
{
    public class TagRepository : ITagRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public TagRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Tag> GetTags()
        {
            return _dbContext.Tags;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
