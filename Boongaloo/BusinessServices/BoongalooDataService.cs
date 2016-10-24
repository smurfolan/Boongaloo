using System;
using System.Collections.Generic;
using DataModel;
using DataModel.UnitOfWork;
using BusinessEntities;
using System.Linq;

namespace BusinessServices
{
    public class BoongalooDataService : IBoongalooDataService
    {
        private readonly BoongalooUoW _unitOfWork;

        public BoongalooDataService()
        {
            _unitOfWork = new BoongalooUoW();
        }

        public IEnumerable<AreaDto> GetAllAreas()
        {
            return this._unitOfWork.AreaRepository.GetAreas()
                .Select(a => new AreaDto() { Id = a.Id, Latitude = a.Latitude, Longitude = a.Longitude})
                .ToList();
        }
    }
}
