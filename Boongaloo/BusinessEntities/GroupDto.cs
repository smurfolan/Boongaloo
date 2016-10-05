using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class GroupDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<AreaDto> Areas { get; set; }   
        public ICollection<TagDto> Tags { get; set; }
        public ICollection<UserDto> Users { get; set; }
    }
}
