using System;
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

        // GET /api/v1/areas/342342
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            throw new NotImplementedException();
        }

        // GET /api/v1/areas/34.234456/42.234/
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
    }
}
