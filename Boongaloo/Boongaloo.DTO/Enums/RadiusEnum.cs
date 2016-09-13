using System.ComponentModel;

namespace Boongaloo.DTO.Enums
{
    public enum RadiusEnum
    {
        [Description("Eye-sight reachable area")]
        FiftyMeters = 50,
        [Description("Stadium, concert hall, small-sized park, Mall, university faculty")]
        HunderdAndFiftyMeters = 150,
        [Description("Small neighbourhood, middle-to-big hospital")]
        ThreeHundredMeters = 300,
        [Description("Middle-sized neighbourhood, small village")]
        FiveHundredMeters = 500
    }
}
