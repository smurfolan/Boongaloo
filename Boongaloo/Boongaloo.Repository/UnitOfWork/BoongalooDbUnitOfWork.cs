using System;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Repositories;

namespace Boongaloo.Repository.UnitOfWork
{
    public class BoongalooDbUnitOfWork : IDisposable
    {
        private readonly BoongalooDbContext _dbContext = new BoongalooDbContext();

        // Initialize these from a constructor using DI
        private AreaRepository areaRepository;
        private GroupRepository groupRepository;
        private UserRepository userRepository;
        private TagRepository tagRepository;
        private LanguageRepository languageRepository;

        public AreaRepository AreaRepository
        {
            get
            {
                if(areaRepository == null)
                    return new AreaRepository(_dbContext);

                return areaRepository;
            }
        }
        public GroupRepository GroupRepository
        {
            get
            {
                if(groupRepository == null)
                    return new GroupRepository(_dbContext);

                return groupRepository;
            }
        }
        public UserRepository UserRepository
        {
            get
            {
                if(userRepository == null)
                    return new UserRepository(_dbContext);

                return userRepository;
            }
        }
        public TagRepository TagRepository
        {
            get
            {
                if (tagRepository == null)
                    return new TagRepository(_dbContext);

                return tagRepository;
            }
        }
        public LanguageRepository LanguageRepository
        {
            get
            {
                if (languageRepository == null)
                    return new LanguageRepository(_dbContext);

                return languageRepository;
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
