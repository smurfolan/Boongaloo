using System.Collections.Generic;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class GroupResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<AreaDto> Areas { get; set; }
        public IEnumerable<UserDto> Users { get; set; } 
    }
}
