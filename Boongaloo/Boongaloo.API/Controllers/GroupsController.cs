using System;
using System.Web.Http;
using Boongaloo.API.Helpers;
using BusinessEntities;
using BusinessServices;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        private readonly IBoongalooService _boongalooServices;

        public GroupsController( /*Comma separated arguments of type interface*/)
        {
            _boongalooServices = new BoongalooService();
            // Handle assignment by DI
        }

        /// <summary>
        /// Example: GET /api/v1/groups/{lat:double}/{lon:double}/
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns>All the groups that contain this point(lat/lon) as part of their diameter</returns>
        [HttpGet]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Get(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = this._boongalooServices.GetGroupsAroundCoordinates(lat, lon);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting groups around coordinates.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST /api/v1/groups/
        /// </summary>
        /// <param name="newGroup">Body sample:{'Name':'Second floor cooks', 'TagIds':[4,1], 'UserIds':[1], 'AreaIds': [1,2]}</param>
        /// <returns>HTTP Code 201 if successfuly created and 500 if not.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] GroupDto newGroup)
        {
            try
            {
                var newlyCreatedGroupId = this._boongalooServices.CreateNewGroup(newGroup);
                return Created("Success", "api/v1/groups/" + newlyCreatedGroupId);
            }
            catch(Exception ex)
            {
                BoongalooApiLogger.LogError("Error while creating new group.", ex);
                return InternalServerError();
            }   
        }

        /// <summary>
        /// Example: POST /api/v1/groups/AsNewArea
        /// </summary>
        /// <param name="newGroup">Body sample:{'Name':'Second floor cooks', 'TagIds':[4,1], 'UserIds':[1],'Latitude':42.657064, 'Longitude':23.28539, 'RadiusId':4}</param>
        /// <returns>Uniqe identifier of the newly created group entity</returns>
        [HttpPost]
        [Route("AsNewArea")]
        public IHttpActionResult Post([FromBody] GroupAsNewAreaDto newGroup)
        {
            try
            {
                var newlyCreatedGroupId = this._boongalooServices.CreateNewGroupAsNewArea(newGroup);
                return Created("Success", "api/v1/groups/" + newlyCreatedGroupId);
            }
            catch(Exception ex)
            {
                BoongalooApiLogger.LogError("Error while creating new group.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: GET /api/v1/groups/{id:int}
        /// </summary>
        /// <param name="id">Unique identifier of a group</param>
        /// <returns>Specific group by its id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = this._boongalooServices.GetGroupById(id);

            return Ok(result);
        }

        /// <summary>
        /// Example: GET api/v1/groups/{id:int}/users
        /// </summary>
        /// <param name="id">Unique identifier of the group you are getting the users from</param>
        /// <returns>All the users for a specific group</returns>
        [HttpGet]
        [Route("{id:int}/users")]
        public IHttpActionResult GetUsers(int id)
        {
            try
            {
                var result = this._boongalooServices.GetUsersForGroup(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting users for group.", ex);
                return InternalServerError();
            }
        }
    };
}
