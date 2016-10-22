using System.Collections.Generic;
using Boongaloo.DTO.Enums;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class AreaResponseDto
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RadiusRangeEnum Radius { get; set; }

        public IEnumerable<GroupResponseDto> Groups { get; set; } 
    }
}
