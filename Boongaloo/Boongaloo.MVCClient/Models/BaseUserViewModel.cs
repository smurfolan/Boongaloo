using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boongaloo.MVCClient.Models
{
    public class BaseUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string SkypeName { get; set; }
        public string Email { get; set; }
    }
}