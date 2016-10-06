using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModel.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private BoongalooDm _dbContext;

        private bool _disposed = false;

        public AreaRepository(BoongalooDm dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<Area> GetAreas()
        {
            return _dbContext.Areas;
        }

        public Area GetAreaById(int areaId)
        {
            return _dbContext.Areas.FirstOrDefault(area => area.Id == areaId);
        }

        public IEnumerable<Area> GetAreasForGroupId(int groupId)
        {
            throw new NotImplementedException();
        }

        public void InsertArea(Area area)
        {
            this._dbContext.Areas.Add(area);
        }

        public void DeleteArea(int areaId)
        {
            throw new NotImplementedException();
        }

        public void UpdateArea(Area area)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Area> GetAreas(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
