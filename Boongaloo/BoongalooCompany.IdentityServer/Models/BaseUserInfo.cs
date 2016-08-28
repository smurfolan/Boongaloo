using System.Collections.Generic;
using System.Web.Mvc;

namespace BoongalooCompany.IdentityServer.Models
{
    public class BaseUserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}