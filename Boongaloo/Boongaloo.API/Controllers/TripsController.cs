﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Boongaloo.API.Helpers;
using Boongaloo.API.UnitOfWork.Trip;

namespace Boongaloo.API.Controllers
{


    [Authorize]
    [EnableCors("https://localhost:44316", "*", "GET, POST, PATCH")]
    public class TripsController : ApiController
    {

        // anyone can get trips
        [Route("api/trips")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new GetTrips(ownerId))
                {
                    var uowResult = uow.Execute();

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

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


        [Route("api/trips/{tripId}")]
        [HttpGet]
        public IHttpActionResult Get(Guid tripId)
        {
            try
            {
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new GetTrip(ownerId, tripId))
                    {
                        var uowResult = uow.Execute();

                        switch (uowResult.Status)
                        {
                            case UnitOfWork.UnitOfWorkStatus.Ok:
                                return Ok(uowResult.Result);

                            case UnitOfWork.UnitOfWorkStatus.NotFound:
                                return NotFound();

                            case UnitOfWork.UnitOfWorkStatus.Forbidden:
                                return  StatusCode(HttpStatusCode.Forbidden);
                            
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



        [Authorize(Roles="PayingUser")]
        [Route("api/trips")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]DTO.TripForCreation tripForCreation)
        {
            try
            {                 
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new CreateTrip(ownerId))
                {
                    var uowResult = uow.Execute(tripForCreation);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Created<DTO.Trip>
                            (Request.RequestUri + "/" + uowResult.Result.Id.ToString(), uowResult.Result);

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

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
 

        [Route("api/trips/{tripId}")]
        [HttpPatch]
        public IHttpActionResult Patch(Guid tripId,
            [FromBody]Marvin.JsonPatch.JsonPatchDocument<DTO.Trip> tripPatchDocument)
        {

            try
            {

                // is the user allowed to update THIS trip? => check in UnitOfWork
                string ownerId = TokenIdentityHelper.GetOwnerIdFromToken();

                using (var uow = new PartiallyUpdateTrip(ownerId, tripId))
                {
                    var uowResult = uow.Execute(tripPatchDocument);

                    switch (uowResult.Status)
                    {
                        case UnitOfWork.UnitOfWorkStatus.Ok:
                            return Ok(uowResult.Result);

                        case UnitOfWork.UnitOfWorkStatus.Invalid:
                            return BadRequest();

                        case UnitOfWork.UnitOfWorkStatus.Forbidden:
                            return StatusCode(HttpStatusCode.Forbidden);

                        case UnitOfWork.UnitOfWorkStatus.NotFound:
                            return NotFound();

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

