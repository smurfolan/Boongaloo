using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Boongaloo.API.Controllers
{
    public class GroupsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetGroupsForCoordinates(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IHttpActionResult CreateNewGroup(double lattitude, double longitude)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public IHttpActionResult GetGroupById(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
