using System;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.UnitOfWork;
using BusinessEntities;
using BusinessServices;
using System.Collections.Generic;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;
        private readonly IBoongalooService _boongalooServices;

        public GroupsController( /*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
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
        /// <param name="newGroup">{'NewAreaGroup':bool, 'Name':string, 'Tags':[{TagEnum}], 'Latitutude':double?, 'Longtitude':double?, 'Radius': RadiusEnum?, 'AreaIds': IEnumerable(int)}
        /// NOTE: TagEnum?->Nullable, enumerable type. Possible values: Help(0), Sport(1), Fun(2), Dating(3)
        /// NOTE: RadiusEnum->Nullable, enumerable type. Possible values: FiftyMeters(50), HunderdAndFiftyMeters(150), ThreeHundredMeters(300), FiveHundredMeters(500)
        /// NOTE: 'Latitutude', 'Longtitude', 'Radius' MUST be NULL if you are just joining to existing areas(NewAreaGroup=false)
        /// NOTE: AreaIds MUST be empty/null if you are creating a new area(NewAreaGroup=true)</param>
        /// <returns>HTTP Code 201 if successfuly created and 500 if not.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] GroupDto newGroup)/*Name, TagIds, AreaIds*/
        {
            try
            {
                var newlyCreatedGroupId = this._boongalooServices.CreateNewGroup(newGroup);
                return Created("api/v1/groups/" + newlyCreatedGroupId, newlyCreatedGroupId);
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
        /// <param name="newGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AsNewArea")]
        public IHttpActionResult Post([FromBody] GroupAsNewAreaDto newGroup)/*latitude, longitude, radiusId*/
        {
            try
            {
                var newlyCreatedGroupId = this._boongalooServices.CreateNewGroupAsNewArea(newGroup);
                return Created("api/v1/groups/" + newlyCreatedGroupId, newlyCreatedGroupId);
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
