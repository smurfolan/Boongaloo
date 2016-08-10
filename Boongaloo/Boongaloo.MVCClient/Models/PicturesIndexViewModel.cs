using System;
using System.Collections.Generic;
using Boongaloo.DTO;

namespace Boongaloo.MVCClient.Models
{
    public class PicturesIndexViewModel
    {
        public List<Picture> Pictures { get; set; }

        public Guid TripId { get; set; }

        public PicturesIndexViewModel(List<Picture> pictures, Guid tripId)
        {
            Pictures = pictures;
            TripId = tripId;
        }
    }
}
