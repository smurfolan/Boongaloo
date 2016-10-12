using System;
using System.Web.Http;
using Boongaloo.API.Helpers;
using BusinessEntities;
using BusinessServices;
using System.Collections.Generic;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/areas")]
    public class AreaController : ApiController
    {
        private readonly IBoongalooService _productServices;

        public AreaController()
        {
            _productServices = new BoongalooService();
        }
        
        /// <summary>
        /// Example: GET /api/v1/areas/{id:int}
        /// </summary>
        /// <param name="id">Id of the area.</param>
        /// <returns>Returns area by its id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var somerEntity = this._productServices.GetAreaById(id);

                var result = this._productServices.GetAreaById(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting area by its id.", ex);
                return InternalServerError();
            }
            
        }

        /// <summary>
        /// Example: GET /api/v1/areas/{lat:double}/{lon:double}/
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns>All the areas around coordinates.</returns>
        [HttpGet]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Get(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result =  this._productServices.GetAreasForCoordinates(lat, lon);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting groups around coordinates.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: GET api/v1/areas/{id:int}/users
        /// </summary>
        /// <param name="id">Id of the area.</param>
        /// <returns>All the users falling into specific area</returns>
        [HttpGet]
        [Route("{id:int}/users")]
        public IHttpActionResult GetUsers(int id)
        {
            try
            {
                var result = this._productServices.GetUsersFromArea(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting users for area.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST api/v1/areas
        /// </summary>
        /// <param name="area">The are we are currently creating</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]AreaDto area)
        {
            try
            {

                var result = this._productServices.GetGroupById(0);

                // The way we add a group to existing areas with tags and name
                var toPostIt = new GroupDto()
                {
                    Name = "New group name",
                    Areas = new List<AreaDto>()
                    {
                        new AreaDto()
                        {
                            Latitude = 33,
                            Longitude = 43,
                            RadiusId = 3
                        }
                    },
                    TagIds = new List<long> { 4, 3 }
                };

                this._productServices.CreateNewGroup(toPostIt);


                this._productServices.CreateNewArea(area);
                return Created("api/v1/areas/" + area.Id, area.Id);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while creating new area.", ex);
                return InternalServerError();
            }
        }
    }
}
