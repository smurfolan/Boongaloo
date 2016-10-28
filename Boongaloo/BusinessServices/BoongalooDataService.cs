using System.Collections.Generic;
using DataModel.UnitOfWork;
using BusinessEntities;
using System.Linq;

namespace BusinessServices
{
    public class BoongalooDataService : IBoongalooDataService
    {
        private readonly BoongalooUnitOfWork _unitOfWork;

        public BoongalooDataService()
        {
            _unitOfWork = new BoongalooUnitOfWork();
        }

        public IEnumerable<MyEntityDto> GetAllMyEntities()
        {
            return this._unitOfWork.MyEntityRepository.GetMyEntities()
                .Select(a => new MyEntityDto() { Id = a.Id, MyColumn = a.MyColumn})
                .ToList();
        }
    }
}
