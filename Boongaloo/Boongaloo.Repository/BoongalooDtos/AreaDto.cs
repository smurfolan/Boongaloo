using Boongaloo.DTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class AreaDto
    {
        public long Id { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [EnumDataType(typeof(RadiusRangeEnum))]
        public RadiusRangeEnum Radius { get; set; }
    }
}
