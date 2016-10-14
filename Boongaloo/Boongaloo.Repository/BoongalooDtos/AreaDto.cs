using Boongaloo.DTO.Enums;
namespace Boongaloo.Repository.BoongalooDtos
{
    public class AreaDto
    {
        public long Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RadiusRangeEnum Radius { get; set; }
    }
}
