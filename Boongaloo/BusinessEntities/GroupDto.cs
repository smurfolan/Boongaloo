using System.Collections.Generic;

namespace BusinessEntities
{
    public class GroupDto : BaseGroupDto
    {
        public long Id { get; set; }

        public ICollection<AreaDto> Areas { get; set; }   
        public ICollection<long> AreaIds { get; set; }

        public ICollection<TagDto> Tags { get; set; }

        public ICollection<UserDto> Users { get; set; }
    }
}
