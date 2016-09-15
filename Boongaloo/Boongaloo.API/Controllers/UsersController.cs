using System;
using System.Linq;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        public UsersController(/*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            // Handle assignment by DI
        }

        /// <summary>
        /// Example: GET api/v1/users/1
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
        /// <param name="userToGroupsModel">The model contains userId and list of pairs groupId-SubscribtionType that indicate if you are un/subscribing</param>
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
                    .UpdateUserSubscriptionsToGroups(userToGroupsModel.UserId, userToGroupsModel.GroupsSubscriptions);

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
        /// <param name="newUser"></param>
        /// <returns>Http status code 201 if user was succesfuly created or 500 if error has occured.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]User newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (this._unitOfWork.UserRepository
                    .GetUsers()
                    .Any(x => x.IdsrvUniqueId == newUser.IdsrvUniqueId || x.Email == newUser.Email))
                    return BadRequest();

                this._unitOfWork.UserRepository.InsertUser(newUser);
                this._unitOfWork.Save();

                return Created("users", newUser);/*TODO: Investigate what should be returned here in the args.*/
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while inserting a new user.", ex);
                return InternalServerError();
            }
        }
    }
}
