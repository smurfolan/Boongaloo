using System.Collections.Generic;
using Boongaloo.DTO.Enums;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class GroupAsNewAreaRequestDto
    {
        public string Name { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<int> AreaIds { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RadiusRangeEnum Radius { get; set; }
    }
}
