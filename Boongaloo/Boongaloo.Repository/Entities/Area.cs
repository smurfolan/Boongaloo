using Boongaloo.DTO.Enums;

namespace Boongaloo.Repository.Entities
{
    public class Area
    {
        public int Id { get; set; }
        public RadiusEnum Radius { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
