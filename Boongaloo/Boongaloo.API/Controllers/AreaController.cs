using System;
using System.Linq;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.UnitOfWork;
using Boongaloo.Repository.BoongalooDtos;
using AutoMapper;
using Boongaloo.API.Automapper;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/areas")]
    public class AreaController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreaController()
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<BoongalooProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();
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

            var result = this._unitOfWork.AreaRepository.GetAreas().FirstOrDefault(x => x.Id == id);

            return Ok(result);
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
        /// Example: POST api/v1/areas
        /// </summary>
        /// <param name="area">Sample post: {'radius':50, 'latitude': 23.1233123,'longitude': 43.1231232}</param>
        /// <returns>HTTP Status of 201 code if area was successfuly created.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]AreaDto area)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var areaAsEntity = this._mapper.Map<AreaDto, Repository.Entities.Area>(area);

                this._unitOfWork.AreaRepository.InsertArea(areaAsEntity);
                this._unitOfWork.Save();

                return Created("Success", "api/v1/areas/" + areaAsEntity.Id);
            }
            catch (Exception ex)
            {
                BoongalooApiLogger.LogError("Error while creating new area.", ex);
                return InternalServerError();
            }
        }
    }
}
