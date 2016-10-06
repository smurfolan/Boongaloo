using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class BoongalooService : IBoongalooService
    {
        private readonly BoongalooUoW _unitOfWork;

        private readonly IMapper _mapper;

        public BoongalooService()
        {
            _unitOfWork = new BoongalooUoW();
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        public AreaDto GetAreaById(int areaId)
        {
            var area = _unitOfWork.AreaRepository.GetAreaById(areaId);

            if (area == null)
                return null;

            var areaAsDto = this._mapper.Map<Area, AreaDto>(area);

            return areaAsDto;
        }

        public IEnumerable<AreaDto> GetAllAreas()
        {
            throw new NotImplementedException();
        }

        public void CreateNewArea(AreaDto area)
        {
            if (area == null)
                throw new ArgumentException("The argument passed to CreateNewArea is null");

            var areaAsEntity = this._mapper.Map<AreaDto, Area>(area);

            this._unitOfWork.AreaRepository.InsertArea(areaAsEntity);

            this._unitOfWork.Save();
        }

        public IEnumerable<AreaDto> GetAreasForCoordinates(double lat, double lon)
        {
            var currentUserLocation = new GeoCoordinate(lat, lon);

            var areaEntities = this._unitOfWork.AreaRepository.GetAreas().ToList()
                .Where(x => currentUserLocation.GetDistanceTo(new GeoCoordinate(x.Latitude, x.Longitude)) <= x.Radius.Range);

            var result = this._mapper.Map<IEnumerable<Area>, IEnumerable<AreaDto>>(areaEntities);

            return result;
        }
    }
}
