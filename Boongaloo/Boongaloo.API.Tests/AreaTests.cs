using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boongaloo.Repository.UnitOfWork;
using Boongaloo.Repository.Entities;
using System.Linq;

namespace Boongaloo.API.Tests
{
    [TestClass]
    public class AreaTests
    {
        private BoongalooDbUnitOfWork uow = new BoongalooDbUnitOfWork(
            "test_groupStore.json",
            "test_areaStore.json", 
            "test_userStore.json", 
            "test_areaGroupBridgeStore.json", 
            "test_groupUserBridgeStore.json",
            "test_groupTagBridgeStore.json",
            "test_userLanguageBridgeStore.json");
            

        [TestMethod]
        public void Area_Gets_Successsfully_Added()
        {
            var areas = this.uow.AreaRepository.GetAreas();
            var nextAreaId = areas.Count() > 0 ? areas.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1 : 1;

            // Arrange
            var newAreaEntity = new Area()
            {
                Latitude = 34.343434,
                Longitude = 21.212121,
                Radius = DTO.Enums.RadiusRangeEnum.FiftyMeters
            };

            // Act
            this.uow.AreaRepository.InsertArea(newAreaEntity);
            this.uow.Save();

            // Assert
            var newlyAddedArea = this.uow.AreaRepository.GetAreas().FirstOrDefault(x => x.Id == nextAreaId);
            Assert.IsNotNull(newlyAddedArea);
            Assert.AreEqual(newlyAddedArea.Id, nextAreaId);
            Assert.AreEqual(newlyAddedArea.Latitude, 34.343434);
            Assert.AreEqual(newlyAddedArea.Longitude, 21.212121);
            Assert.AreEqual(newlyAddedArea.Radius, DTO.Enums.RadiusRangeEnum.FiftyMeters);
        }
    }
}
