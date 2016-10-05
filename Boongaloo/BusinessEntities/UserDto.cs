using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class UserDto
    {
        public long Id { get; set; }

        public long IdSrvId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string About { get; set; }

        public long GenderId { get; set; }

        public DateTime? BirthDate { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public ICollection<GroupDto> Groups { get; set; }
        
        public ICollection<LanguageDto> Languages { get; set; }
    }
}
