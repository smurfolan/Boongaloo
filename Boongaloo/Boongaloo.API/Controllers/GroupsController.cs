using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.DTO.Enums;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        public GroupsController( /*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
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
                //var result = this._unitOfWork.GroupRepository.GetGroups(lat, lon);

                return Ok(/*result*/);
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
        public IHttpActionResult Post([FromBody] GroupDto newGroup)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //var newGroupEntity = new Group() {Name = newGroup.Name, Tags = newGroup.Tags};

                //if (newGroup.NewAreaGroup && newGroup.AreaIds == null)
                //{
                //    this._unitOfWork.AreaRepository.InsertArea(new Area()
                //    {
                //        Latitude = newGroup.Latitutude.Value,
                //        Longitude = newGroup.Longtitude.Value,
                //        Radius = (RadiusEnum) newGroup.Radius
                //    });

                //    this._unitOfWork.GroupRepository.InsertGroup(
                //        newGroupEntity,
                //        new List<int>()
                //        {
                //            this._unitOfWork.AreaRepository.GetAreas().Count()
                //        });
                //}
                //else
                //{
                //    this._unitOfWork.GroupRepository.InsertGroup(newGroupEntity, newGroup.AreaIds);
                //}

                //this._unitOfWork.Save();

                return Created("groups", ""/*newGroupEntity*/);
                /*TODO: Investigate what should be returned here in the args.*/
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while inserting a new group.", ex);
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

            //var result = _unitOfWork.GroupRepository.GetGroupById(id);

            return Ok(/*result*/);
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
                //var result = this._unitOfWork.UserRepository.GetUsersFromGroup(id);
                return Ok(/*result*/);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting users for group.", ex);
                return InternalServerError();
            }
        }
    };
}
