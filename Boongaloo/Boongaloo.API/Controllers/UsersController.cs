using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using Boongaloo.Repository.Entities;
using Boongaloo.Repository.UnitOfWork;

namespace Boongaloo.API.Controllers
{
    [Authorize]
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

            var result = this._unitOfWork.UserRepository.GetUsers().FirstOrDefault(x => x.Id == id);

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]User newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (this._unitOfWork.UserRepository
                    .GetUsers()
                    .Any(x => x.IdsrvUniqueId == newUser.IdsrvUniqueId || x.Email == newUser.Email))
                    return BadRequest();

                this._unitOfWork.UserRepository.InsertUser(newUser);
                this._unitOfWork.Save();

                return Created("users", newUser);
                //return CreatedAtRoute("api/v1/users", new { id = newUser.IdsrvUniqueId }, newUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while inserting a new user. More:" + ex.Message);
                return InternalServerError();
            }
        }
    }
}
