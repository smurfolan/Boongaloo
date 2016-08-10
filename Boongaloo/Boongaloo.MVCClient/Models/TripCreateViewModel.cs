using System.Web;
using Boongaloo.DTO;

namespace Boongaloo.MVCClient.Models
{
    public class TripCreateViewModel
    {
         
        public HttpPostedFileBase MainImage { get; set; }

        public TripForCreation Trip { get; set; }

        public TripCreateViewModel()
        {

        }

        public TripCreateViewModel(TripForCreation trip)
        {
            Trip = trip;
        }
    }
}
