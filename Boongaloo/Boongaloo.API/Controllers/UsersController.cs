using System;
using System.Web.Http;
using Boongaloo.API.Helpers;
using BusinessServices;
using BusinessEntities;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private readonly IBoongalooService _boongalooServices;

        public UsersController(/*Comma separated arguments of type interface*/)
        {
            _boongalooServices = new BoongalooService();
            // Handle assignment by DI
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

            var result = this._boongalooServices.GetUserById(id);

            return Ok(result);
        }

        /// <summary>
        /// Example: POST api/v1/users
        /// </summary>
        /// <param name="newUser">Body sample: {'IdSrvId':'asd79879s87d', 'FirstName': 'Mitko', 'LastName': 'Stefchev', 'Email':'mit@ko.com'}</param>
        /// <returns>Http status code 201 if user was succesfuly created or 500 if error has occured.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]UserDto newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var newlyCreatedUserId = this._boongalooServices.CreateNewUser(newUser);

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
        /// <param name="updatedUserData">Body sample: {'IdSrvId':'asd79879s87d', 'FirstName': 'Mitko', 'LastName': 'Stefchev', 'Email':'mit@ko.com', 'About': 'If i was about to', 'GenderId': 2, 'BirthDate': '2/13/2009 12:00:00', 'LanguageIds':[2,4]}</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]UserDto updatedUserData)
        {
            var requiredUser = this._boongalooServices.GetUserById(id);

            if (!ModelState.IsValid || requiredUser == null)
                return BadRequest();

            try
            {
                this._boongalooServices.UpdateUser(id, updatedUserData);

                return Ok();
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while updating user.", ex);
                return InternalServerError();
            }
        }
    }
}
