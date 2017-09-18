using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boongaloo.MvcClient.AuthCode.SignalR.DTOs
{
    public class SRGroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Guid> UserIds { get; set; }
        public IEnumerable<Guid> TagIds { get; set; }
    }
}