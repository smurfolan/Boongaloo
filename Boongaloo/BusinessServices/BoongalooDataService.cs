using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class BoongalooDataService : IBoongalooDataService
    {
        private readonly BoongalooUoW _unitOfWork;

        public BoongalooDataService()
        {
            _unitOfWork = new BoongalooUoW();
        }
    }
}
