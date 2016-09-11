using System;
using System.Net;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Repositories;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        public GroupsController(/*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            // Handle assignment by DI
        }

        // GET /api/v1/groups/34.234456/42.234/
        /// <summary>
        /// Returns all the groups that contain this point(lat/lon) as part of their diameter 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Get(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Content(HttpStatusCode.OK, "You successfuly extracted all groups around coordiantes");
        }

        // POST /api/v1/groups/34.234456/42.234/
        /// <summary>
        /// Creates a new group centered with the coordinates that were passed.
        /// </summary>
        /// <param name="newGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Post(Group newGroup)/*double lat, double lon*/
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            throw new NotImplementedException();
        }

        // GET /api/v1/groups/342342
        /// <summary>
        /// Extracts specific group by its id.
        /// </summary>
        /// <param name="id">Unique identifier of a group</param>
        /// <returns>A single group object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _unitOfWork.GroupRepository.GetGroupById(id);

            return Ok(result);
        }
    }
}
