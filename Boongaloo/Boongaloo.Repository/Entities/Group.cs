using System.Collections.Generic;
using System.ComponentModel;

namespace Boongaloo.Repository.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TagEnum> Tags { get; set; }

        public IEnumerable<User> Users { get; set; }

        public RadiusEnum Radius { get; set; }
    }

    public enum TagEnum
    {
        Help, Sport, Fun, Dating
    }

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
