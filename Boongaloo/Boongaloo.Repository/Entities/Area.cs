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
        public int Radius { get; set; }

        // This is supposed to be DbGeography object cast to string representation
        public string Center { get; set; }
    }
}
