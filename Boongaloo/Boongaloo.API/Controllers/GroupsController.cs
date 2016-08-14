﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [HttpGet]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Get(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            throw new NotImplementedException();
        }

        // POST /api/v1/groups/34.234456/42.234/
        [HttpPost]
        [Route("{lat:double}/{lon:double}")]
        public IHttpActionResult Post(double lat, double lon)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            throw new NotImplementedException();
        }

        // GET /api/v1/groups/342342
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            throw new NotImplementedException();
        }
    }
}
