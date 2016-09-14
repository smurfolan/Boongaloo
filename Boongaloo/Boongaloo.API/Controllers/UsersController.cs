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

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = this._unitOfWork.UserRepository.GetUsers().FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }

        // Get all users for a group 
        // Get all users for an area

        [HttpPost]
        [Route("ChangeGroupsSubscribtion")]
        public IHttpActionResult Post([FromBody]RelateUserToGroupsDto userToGroupsModel)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            try
            {
                if (userToGroupsModel.SubscribeRequest)
                {
                    this._unitOfWork.UserRepository
                        .SubscribeUserForGroups(userToGroupsModel.UserId, userToGroupsModel.GroupIds);
                }
                else
                {
                    this._unitOfWork.UserRepository
                        .UnsubscribeUserFromGroups(userToGroupsModel.UserId, userToGroupsModel.GroupIds);
                }
                
                this._unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while subscribing user to groups.", ex);
                return InternalServerError();
            }
        }

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
