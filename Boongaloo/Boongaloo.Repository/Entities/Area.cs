using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boongaloo.Repository.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public RadiusEnum Radius { get; set; }

        // It has to be stored in a 'well known' format for DbGeography. POINT("", "")
        public string Center { get; set; }
    }
}
