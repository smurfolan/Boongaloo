using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Tests
{
    public class BaseTest
    {
        protected BoongalooDbUnitOfWork uow = new BoongalooDbUnitOfWork(
            "test_groupStore.json",
            "test_areaStore.json",
            "test_userStore.json",
            "test_userNotificationSettingsStore.json",
            "test_areaGroupBridgeStore.json",
            "test_groupUserBridgeStore.json",
            "test_groupTagBridgeStore.json",
            "test_userLanguageBridgeStore.json");
    }
}
