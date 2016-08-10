using System;
using System.Web;
using Boongaloo.DTO;

namespace Boongaloo.MVCClient.Models
{
    public class PictureCreateViewModel
    {
        public HttpPostedFileBase PictureFile { get; set; }
        public PictureForCreation Picture { get; set; }

        public Guid TripId { get; set; }

        public PictureCreateViewModel()
        {

        }

        public PictureCreateViewModel(PictureForCreation picture, Guid tripId)
        {
            Picture = picture;
            TripId = tripId;
        }
    }
}
