using System.Collections.Generic;
using System.Linq;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;
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

        [TestMethod]
        public void New_User_Id_Is_Returned_When_New_User_Created()
        {
            var latestUserRecord = this.uow.UserRepository.GetUsers().OrderBy(x => x.Id).LastOrDefault();
            var nextUserId = latestUserRecord != null ? latestUserRecord.Id + 1 : 1;

            // Arrange
            var newUser = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
            };

            // Act
            var newlyCreatedUserId = this.uow.UserRepository.InsertUser(newUser);
            this.uow.Save();

            // Assert
            Assert.IsNotNull(newlyCreatedUserId);
            Assert.AreEqual(newlyCreatedUserId, nextUserId);
        }

        [TestMethod]
        public void New_User_Subscription_Removes_Old_Subsribtions_That_Are_Not_Amongst_New_Ones()
        {
            // Arrange
            var firstNewGroup = new Group(){Name = "FirstGroup"};
            var secondNewGroup = new Group(){Name = "SecondGroup"};

            var thirdNewGroup = new Group() { Name = "ThirdGroup" };

            this.uow.GroupRepository.InsertGroup(firstNewGroup);
            this.uow.GroupRepository.InsertGroup(secondNewGroup);
            this.uow.GroupRepository.InsertGroup(thirdNewGroup);

            var firstGroupId = firstNewGroup.Id;
            var secondGroupId = secondNewGroup.Id;
            var thirdGroupId = thirdNewGroup.Id;

            var newUser = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                GroupIds = new List<int>() { firstGroupId, secondGroupId }
            };

            this.uow.UserRepository.InsertUser(newUser);

            this.uow.Save();

            var newUserId =
                this.uow.UserRepository.GetUserByStsId("https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d").Id;

            var userGroups = this.uow.UserRepository.GetUserById(newUserId).Groups.ToList();
            Assert.AreEqual(userGroups.Count(), 2);
            Assert.IsTrue(userGroups.FirstOrDefault(x => x.Id == firstGroupId) != null);
            Assert.IsTrue(userGroups.FirstOrDefault(x => x.Id == secondGroupId) != null);

            // Act
            var newUserSubscription = new RelateUserToGroupsDto()
            {
                UserId = newUserId,
                GroupsUserSubscribes = new List<int>() { thirdGroupId}
            };
            
            this.uow.UserRepository.UpdateUserSubscriptionsToGroups(newUserSubscription.UserId, newUserSubscription.GroupsUserSubscribes);
            this.uow.Save();

            // Assert
            var updatedUserGroups = this.uow.UserRepository.GetUserById(newUserId).Groups.ToList();
            Assert.AreEqual(updatedUserGroups.Count(), 1);
            Assert.IsTrue(updatedUserGroups.FirstOrDefault(x => x.Id == firstGroupId) == null);
            Assert.IsTrue(updatedUserGroups.FirstOrDefault(x => x.Id == secondGroupId) == null);
            Assert.IsTrue(updatedUserGroups.FirstOrDefault(x => x.Id == thirdGroupId) != null);

            var secondUserSubscribtion = new RelateUserToGroupsDto()
            {
                UserId = newUserId,
                GroupsUserSubscribes = new List<int> { thirdGroupId, firstGroupId}
            };

            this.uow.UserRepository.UpdateUserSubscriptionsToGroups(secondUserSubscribtion.UserId, secondUserSubscribtion.GroupsUserSubscribes);
            this.uow.Save();

            var secondTimeUpdatedUserGroups = this.uow.UserRepository.GetUserById(newUserId).Groups.ToList();
            Assert.AreEqual(secondTimeUpdatedUserGroups.Count(), 2);
            Assert.IsTrue(secondTimeUpdatedUserGroups.FirstOrDefault(x => x.Id == thirdGroupId) != null);
            Assert.IsTrue(secondTimeUpdatedUserGroups.FirstOrDefault(x => x.Id == firstGroupId) != null);
        }
    }
}
