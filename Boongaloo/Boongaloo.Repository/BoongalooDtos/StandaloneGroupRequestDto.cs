using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Boongaloo.Repository.BoongalooDtos
{
    public class StandaloneGroupRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IEnumerable<int> TagIds { get; set; }

        [Required]
        public IEnumerable<int> AreaIds { get; set; }  

        public int UserId { get; set; } 
    }
}
