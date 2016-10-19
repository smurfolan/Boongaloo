using Boongaloo.Repository.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boongaloo.API.Tests
{
    [TestClass]
    public class UserTests
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
        public void User_Gets_Successfuly_Created()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
