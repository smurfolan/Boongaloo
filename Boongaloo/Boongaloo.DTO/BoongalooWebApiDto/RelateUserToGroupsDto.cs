using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Boongaloo.DTO.BoongalooWebApiDto
{
    public class RelateUserToGroupsDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public IEnumerable<int> GroupsUserSubscribes { get; set; } 
    }
}
