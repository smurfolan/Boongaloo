using System.Collections.Generic;
using System.Linq;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Boongaloo.DTO.Enums;

namespace Boongaloo.API.Tests
{
    [TestClass]
    public class UserTests
    {
        private BoongalooDbUnitOfWork uow = new BoongalooDbUnitOfWork(
            "test_groupStore.json",
            "test_areaStore.json",
            "test_userStore.json",
            "test_userNotificationSettingsStore.json",
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

        [TestMethod]
        public void User_Can_Be_Successfuly_Received_After_His_Creation()
        {
            // arrange
            var newUserDto = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                FirstName = "Stefcho",
                LastName = "Stefchev",
                Email = "used@to.know",
                About = "Straightforward",
                Gender = DTO.Enums.GenderEnum.Male,
                BirthDate = DateTime.UtcNow,
                PhoneNumber = "+395887647288",
                LanguageIds = new List<int> { 1, 3 },
                GroupIds = new List<int> { 1 }
            };

            // act
            var newlyCreatedUserId = this.uow.UserRepository.InsertUser(newUserDto);
            this.uow.Save();

            var userFromDb = this.uow.UserRepository.GetUserByStsId("https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d");

            // assert
            Assert.AreEqual(newlyCreatedUserId, userFromDb.Id);
        }

        [TestMethod]
        public void User_Gets_Successfully_Updated()
        {
            // arrange

            // act

            // assert
        }

        [TestMethod]
        public void User_Subscribtion_With_No_Groups_Removes_All_Subscriptions_For_User()
        {
            // arrange

            // act

            // assert
        }

        [TestMethod]
        public void User_Can_Update_His_Notification_Settings()
        {
            // arrange
            var newUserToBeAdded = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                FirstName = "Stefcho",
                LastName = "Stefchev"
            };

            var newUserNotificationSettings = new EditUserNotificationsRequestDto()
            {
                AutomaticallySubscribeToAllGroups = false,
                AutomaticallySubscribeToAllGroupsWithTag = true,
                SubscribedTagIds = new List<int>() { (int)TagEnum.Fun, (int)TagEnum.School}
            };

            // act
            var newUserId = this.uow.UserRepository.InsertUser(newUserToBeAdded);
            this.uow.Save();

            // assert
            var settingsBeforeUpdate =
                this.uow.UserNotificationSettingsRepository.GetNotificationSettingsForUserWithId(newUserId);

            Assert.IsNotNull(settingsBeforeUpdate);
            Assert.IsTrue(settingsBeforeUpdate.AutomaticallySubscribeToAllGroups);
            Assert.IsFalse(settingsBeforeUpdate.AutomaticallySubscribeToAllGroupsWithTag);
            Assert.AreEqual(settingsBeforeUpdate.SubscribedTagIds.Count(), 0);

            this.uow.UserNotificationSettingsRepository.UpdateUserNotificationSettings(newUserId, newUserNotificationSettings);

            settingsBeforeUpdate = this.uow.UserNotificationSettingsRepository.GetNotificationSettingsForUserWithId(newUserId);

            Assert.IsFalse(settingsBeforeUpdate.AutomaticallySubscribeToAllGroups);
            Assert.IsTrue(settingsBeforeUpdate.AutomaticallySubscribeToAllGroupsWithTag);
            Assert.AreEqual(settingsBeforeUpdate.SubscribedTagIds.Count(), 2);
        }

        [TestMethod]
        public void User_Can_Get_His_Notification_Settings()
        {
            // arrange
            var newUserToBeAdded = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                FirstName = "Stefcho",
                LastName = "Stefchev"
            };

            // act
            var newUserId = this.uow.UserRepository.InsertUser(newUserToBeAdded);
            this.uow.Save();

            var notificationsForUser = this.uow.UserNotificationSettingsRepository.GetNotificationSettingsForUserWithId(newUserId);

            // assert
            Assert.IsNotNull(notificationsForUser);
        }

        [TestMethod]
        public void Default_User_Notification_Settings_Are_Created_On_First_User_Login()
        {
            // arrange
            var newUserToBeAdded = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                FirstName = "Stefcho",
                LastName = "Stefchev"
            };

            // act
            var newUserId = this.uow.UserRepository.InsertUser(newUserToBeAdded);
            this.uow.Save();

            // assert
            var notificationSettings = this.uow.UserNotificationSettingsRepository.GetAllUserNotificationSettings().FirstOrDefault(u => u.UserId == newUserId);

            Assert.IsNotNull(notificationSettings);
            Assert.IsTrue(notificationSettings.AutomaticallySubscribeToAllGroups);
            Assert.IsFalse(notificationSettings.AutomaticallySubscribeToAllGroupsWithTag);
            Assert.AreEqual(notificationSettings.SubscribedTagIds.Count(), 0);
        }
    }
}
