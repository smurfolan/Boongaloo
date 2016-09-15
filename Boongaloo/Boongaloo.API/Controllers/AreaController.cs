﻿using System;
using System.Linq;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/areas")]
    public class AreaController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        public AreaController()
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
        }
        
        /// <summary>
        /// Example: GET /api/v1/areas/342342
        /// </summary>
        /// <param name="id">Id of the area.</param>
        /// <returns>Returns area by its id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = this._unitOfWork.AreaRepository.GetAreas().FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// Example: GET /api/v1/areas/34.234456/42.234/
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
                var result = this._unitOfWork.AreaRepository.GetAreas(lat, lon);

                return Ok(result);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while getting groups around coordinates.", ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// Example: GET api/v1/areas/{id}/users
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
    }
}