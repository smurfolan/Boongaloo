using System.Collections.Generic;
using System.Linq;
using Boongaloo.DTO.Enums;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Boongaloo.API.Tests
{
    [TestClass]
    public class GroupTests : BaseTest
    {
        [TestMethod]
        public void Group_As_Part_Of_Other_Areas_Gets_Successfully_Added()
        {
            // arrange
            // Create new area
            int newUserId;
            int nextGroupId;
            var newAreaId = CrateNewGroupWithAreaAndUsers(out newUserId, out nextGroupId);

            // assert
            var areaSupposedToHaveTheNewGroup = this.uow.AreaRepository.GetAreaById(newAreaId);

            Assert.IsNotNull(areaSupposedToHaveTheNewGroup);
            Assert.IsTrue(areaSupposedToHaveTheNewGroup.Groups.Select(g => g.Id).Contains(nextGroupId));
            Assert.IsTrue(areaSupposedToHaveTheNewGroup.Groups.Any(g => g.Users.Select(u => u.Id).Contains(newUserId)));
        }

        [TestMethod]
        public void Group_As_New_Area_Gets_Successfully_Added()
        {
            // Same as above test but in advantage it creates new area.
        }

        [TestMethod]
        public void Group_Returns_All_Users_It_Has()
        {
            // arrange
            int newUserId;
            int nextGroupId;
            var newAreaId = CrateNewGroupWithAreaAndUsers(out newUserId, out nextGroupId);

            // act
            var groupWeJustAdded = this.uow.GroupRepository.GetGroupById(nextGroupId);

            // assert
            Assert.AreEqual(groupWeJustAdded.Users.Count(), 1);
            Assert.IsTrue(groupWeJustAdded.Users.Select(u => u.Id).Contains(newUserId));
        }

        [TestMethod]
        public void Group_Can_Be_Created_As_New_Area()
        {
            // arrange

            // act

            // assert
        }

        [TestMethod]
        public void Groups_Around_Coordinates_Can_Be_Extracted()
        {
            // arrange

            // act

            // assert
        }

        #region Helper methods
        private int CrateNewGroupWithAreaAndUsers(out int newUserId, out int nextGroupId)
        {
            var newAreaEntity = new Area()
            {
                Latitude = 34.343434,
                Longitude = 21.212121,
                Radius = RadiusRangeEnum.FiftyMeters
            };

            var newAreaId = this.uow.AreaRepository.InsertArea(newAreaEntity);
            this.uow.Save();

            // Create new user
            var newUserToBeAdded = new NewUserRequestDto()
            {
                IdsrvUniqueId = "https://boongaloocompanysts/identity78f100e9-9d90-4de8-9d7d",
                FirstName = "Stefcho",
                LastName = "Stefchev"
            };

            newUserId = this.uow.UserRepository.InsertUser(newUserToBeAdded);
            this.uow.Save();

            // Create new group
            var newGroup = new StandaloneGroupRequestDto()
            {
                Name = "Second floor cooks",
                TagIds = new List<int>() { (int)TagEnum.Fun, (int)TagEnum.University },
                AreaIds = new List<int>() { newAreaId },
                UserId = newUserId
            };

            var latestGroupRecord = this.uow.GroupRepository.GetGroups().OrderBy(x => x.Id).LastOrDefault();
            nextGroupId = latestGroupRecord?.Id + 1 ?? 1;

            // act
            this.uow.GroupRepository.InsertGroup(
                new Group() { Id = nextGroupId, Name = "Second floor cooks" },
                newGroup.AreaIds,
                newGroup.TagIds,
                newUserId);

            this.uow.Save();
            return newAreaId;
        }
        #endregion
    }
}
