using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Boongaloo.API.Automapper;
using Boongaloo.API.Helpers;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.BoongalooDtos;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(/*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            // Handle assignment by DI
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// Example: GET api/v1/users/{id:int}
        /// </summary>
        /// <param name="id">Unique identifier of the user. Not the one that comes from identity server.</param>
        /// <returns>User by his id</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = this._unitOfWork.UserRepository.GetUsers().FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// Example: POST api/v1/users/ChangeGroupsSubscribtion
        /// </summary>
        /// <param name="userToGroupsModel">{'UserId':int, 'GroupsSubscriptions':[3, 105]}</param>
        /// <returns>Http.OK if the operation was successful or Http.500 if there was an error.</returns>
        [HttpPost]
        [Route("ChangeGroupsSubscribtion")]
        public IHttpActionResult Post([FromBody]RelateUserToGroupsDto userToGroupsModel)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                this._unitOfWork.UserRepository
                    .UpdateUserSubscriptionsToGroups(userToGroupsModel.UserId, userToGroupsModel.GroupsUserSubscribes);

                this._unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while subscribing user to groups.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST api/v1/users
        /// </summary>
        /// <param name="newUser">{'IdsrvUniqueId' : 'adss23d2s', 'FirstName': 'Stefcho', 'LastName': 'Stefchev', 'Email': 'used@to.know', 'About': 'Straightforward', 'Gender': '0', 'BirthDate': '0001-01-01T00:00:00', 'PhoneNumber': '+395887647288', 'LanguageIds' : [1,3], 'GroupIds': [1]}</param>
        /// <returns>Http status code 201 if user was succesfuly created or 500 if error has occured.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]NewUserRequestDto newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (this._unitOfWork.UserRepository
                    .GetUsers()
                    .Any(x => x.IdsrvUniqueId == newUser.IdsrvUniqueId || x.Email == newUser.Email))
                    return BadRequest();

                var newlyCreatedUserId = this._unitOfWork.UserRepository.InsertUser(newUser);
                //this._unitOfWork.Save();

                return Created("Success", "api/v1/users/" + newlyCreatedUserId);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while inserting a new user.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: PUT api/v1/users/{id:int}
        /// </summary>
        /// <param name="id">Unique identifier of the user that will be updated</param>
        /// <param name="updateUserData">Updated user data</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]NewUserRequestDto updateUserData)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                updateUserData.Id = id;
                this._unitOfWork.UserRepository.UpdateUser(updateUserData);

                this._unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while updating user.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: GET api/v1/users/{id:int}/subscriptions
        /// </summary>
        /// <param name="uid">Updated user data</param>
        /// <returns>List of integers which indicate the group ids to which the user is subscribed</returns>
        [HttpGet]
        [Route("{uid:int}/subscriptions")]
        public IHttpActionResult GetSubscriptions(int uid)
        {
            try
            {
                var result = this._unitOfWork.UserRepository.GetUserSubscriptions(uid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error getting user subscribtions.", ex);
                return InternalServerError();
            }
        }
    }
}
