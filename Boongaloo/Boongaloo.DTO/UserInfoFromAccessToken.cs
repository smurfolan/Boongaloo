using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boongaloo.DTO
{
    public class UserInfoFromAccessToken
    {
        public RolesEnum Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SkypeName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
