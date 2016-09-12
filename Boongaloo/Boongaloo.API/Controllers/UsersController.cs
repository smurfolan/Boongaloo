using System;
using System.Net;
using System.Web.Http;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/v1/users")]
    public class UsersController : ApiController
    {
        private BoongalooDbUnitOfWork _unitOfWork;

        public UsersController(/*Comma separated arguments of type interface*/)
        {
            _unitOfWork = new BoongalooDbUnitOfWork();
            // Handle assignment by DI
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Content(HttpStatusCode.OK, "You successfuly extracted user by its id.");
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]User newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            throw new NotImplementedException();
        }
    }
}
