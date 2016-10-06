﻿using System;
using System.Linq;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.UnitOfWork;
using BusinessEntities;
using BusinessServices;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/areas")]
    public class AreaController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        private readonly IBoongalooService _productServices;

        public AreaController()
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
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
                var result = this._unitOfWork.UserRepository.GetUsersFromArea(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting users for area.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: POST api/v1/areas Content: {'groups':[], 'latitude':32.323232, 'longitude':45.454545, 'radiusId':2}
        /// </summary>
        /// <param name="area">The are we are currently creating</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]AreaDto area)
        {
            try
            {
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
