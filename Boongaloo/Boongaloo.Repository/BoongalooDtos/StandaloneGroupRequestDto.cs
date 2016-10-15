using System.Collections.Generic;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class StandaloneGroupRequestDto
    {
        public string Name { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IEnumerable<int> AreaIds { get; set; }  
        public IEnumerable<int> UserIds { get; set; } 
    }
}
