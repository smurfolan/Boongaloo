using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class BoongalooService : IBoongalooService
    {
        private readonly BoongalooUoW _unitOfWork;

        public BoongalooService()
        {
            _unitOfWork = new BoongalooUoW();
        }

        public AreaDto GetAreaById(int areaId)
        {
            var area = _unitOfWork.AreaRepository.GetAreaById(areaId);

            if (area == null)
                return null;

            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Area, AreaDto>();
            });

            var mapper = config.CreateMapper();

            var areaDtoResult = mapper.Map<Area, AreaDto>(area);
                
            return areaDtoResult;
        }

        public IEnumerable<AreaDto> GetAllAreas()
        {
            throw new NotImplementedException();
        }
    }
}
