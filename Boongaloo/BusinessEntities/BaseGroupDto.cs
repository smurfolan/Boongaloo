using System.Collections.Generic;

namespace BusinessEntities
{
    public class BaseGroupDto
    {
        public string Name { get; set; }
        public ICollection<long> TagIds { get; set; }
    }
}
