using System;
using System.Net;
using System.Web.Http;
using Boongaloo.API.Helpers;
using Boongaloo.Repository.Contexts;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.Repositories;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/groups")]
    public class GroupsController : ApiController
    {
        public GroupsController(/*Comma separated arguments of type interface*/)
        {
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

            try
            {
                using (var groupRepository = new GroupRepository())
                {
                    
                }

                return Content(HttpStatusCode.Created, "You successfuly created new group by passing coordinates");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            
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

            return Content(HttpStatusCode.OK, "You successfuly extracted specific group by its id");
        }
    }
}
