using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Interfaces;

namespace Boongaloo.Repository.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private BoongalooDbContext _dbContext;

        private bool _disposed = false;

        public AreaRepository(BoongalooDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Area> GetAreas()
        {
            return _dbContext.Areas.ToList();
        }
        public Area GetAreaById(int areaId)
        {
            return _dbContext.Areas.FirstOrDefault(x => x.Id == areaId);
        }
        public IEnumerable<Area> GetAreasForGroupId(int groupId)
        {
            return (from item1 in _dbContext.AreaToGroup
                    join item2 in _dbContext.Areas
                    on item1.AreaId equals item2.Id
                    where item1.GroupId == groupId
                    select item2).ToList();
        }

        public void InsertArea(Area area)
        {
            _dbContext.Areas.Add(area);
        }
        public void DeleteArea(int areaId)
        {
            var toBeDeleted = _dbContext.Areas.FirstOrDefault(x => x.Id == areaId);
            _dbContext.Areas.Remove(toBeDeleted);
        }
        public void UpdateArea(Area area)
        {
            var toBeUpdated = _dbContext.Areas.FirstOrDefault(x => x.Id == area.Id);

            if (toBeUpdated == null)
                return;

            toBeUpdated.Center = area.Center;
            toBeUpdated.Radius = area.Radius;
        }

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
