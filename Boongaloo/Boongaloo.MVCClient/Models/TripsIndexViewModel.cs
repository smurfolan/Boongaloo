using System.Collections.Generic;
using Boongaloo.DTO;

namespace Boongaloo.MVCClient.Models
{
    public class TripsIndexViewModel
    {
        public List<Trip> Trips { get; set; }
 


        public TripsIndexViewModel()
        {
            Trips = new List<Trip>();
        }
    }
}
