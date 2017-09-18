using System;
using System.Collections.Generic;

namespace Boongaloo.MvcClient.AuthCode.SignalR.DTOs
{
    public class SRAreaDto
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ApproximateAddress { get; set; }
        public int Radius { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
    }
}