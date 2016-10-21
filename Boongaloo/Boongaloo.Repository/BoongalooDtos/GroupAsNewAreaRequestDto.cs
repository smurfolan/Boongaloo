using System.Collections.Generic;
using Boongaloo.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class GroupAsNewAreaRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<int> AreaIds { get; set; }
        public IEnumerable<int> UserIds { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [EnumDataType(typeof(RadiusRangeEnum))]
        public RadiusRangeEnum Radius { get; set; }
    }
}
