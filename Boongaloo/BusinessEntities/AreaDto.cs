﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class AreaDto
    {
        public long Id { get; set; }
        public long RadiusId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RadiusDto Radius { get; set; }
        public ICollection<GroupDto> Groups { get; set; }
    }
}