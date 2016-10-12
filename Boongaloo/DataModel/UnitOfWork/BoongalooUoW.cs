using System;
using DataModel.Repositories;

namespace DataModel.UnitOfWork
{
    public class BoongalooUoW : IDisposable
    {
        private readonly BoongalooDm _dbContext = new BoongalooDm();

        private AreaRepository areaRepository;
        private GroupRepository groupRepository;
        private UserRepository userRepository;
        private TagRepository tagRepository;
        private RadiusRepository radiusRepository;
        private LanguageRepository languageRepository;

        public AreaRepository AreaRepository
        {
            get
            {
                if (areaRepository == null)
                    return new AreaRepository(_dbContext);

                return areaRepository;
            }
        }
        public GroupRepository GroupRepository
        {
            get
            {
                if (groupRepository == null)
                    return new GroupRepository(_dbContext);

                return groupRepository;
            }
        }
        public UserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
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
        public RadiusRepository RadiusRepository
        {
            get
            {
                if(radiusRepository == null)
                    return new RadiusRepository(_dbContext);

                return radiusRepository;
            }
        }
        public LanguageRepository LanguageRepository
        {
            get
            {
                if (radiusRepository == null)
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
