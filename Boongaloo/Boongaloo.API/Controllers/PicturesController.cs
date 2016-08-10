﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Boongaloo.API.Helpers;
using Boongaloo.API.UnitOfWork.Picture;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [EnableCors("https://localhost:44316", "*", "GET, POST, DELETE")]
    public class PicturesController : ApiController
    {

        [Route("api/trips/{tripId}/pictures")]
        [HttpGet]
        public IHttpActionResult Get(Guid tripId)
        {
            try
            {
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new GetPictures(ownerId, tripId))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        default:
                            return InternalServerError();
                    }
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

   
        [Route("api/trips/{tripId}/pictures")]
        [HttpPost]
        public IHttpActionResult Post(Guid tripId, [FromBody]DTO.PictureForCreation pictureForCreation)
        {
            try
            {

                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new CreatePicture(ownerId, tripId))
                {
                    var uowResult = uow.Execute(pictureForCreation);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Created<DTO.Picture>
                            (Request.RequestUri + "/" + uowResult.Result.Id.ToString(), uowResult.Result);
                            
                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        default:
                            return InternalServerError();
                    }
                }
            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }


        // TODO: is the user allowed to delete?
        [Route("api/trips/{tripId}/pictures/{pictureId}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid tripId, Guid pictureId)
        {
            try
            {
                // the user can delete.  But can he also delete THIS picture?
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new DeletePicture(ownerId, tripId, pictureId))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return StatusCode(HttpStatusCode.NoContent);

                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);
                        default:
                            return InternalServerError();
                    }
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
