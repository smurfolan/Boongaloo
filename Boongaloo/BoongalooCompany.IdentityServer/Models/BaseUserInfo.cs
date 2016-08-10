using System.Collections.Generic;
using System.Web.Mvc;

namespace BoongalooCompany.IdentityServer.Models
{
    public class BaseUserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SkypeName { get; set; }
        public string PhoneNumber { get; set; }

        public string Role { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public BaseUserInfo()
        {
            Roles = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Recruiter", Value = "Recruiter"},
                new SelectListItem() { Text = "Job Applicant", Value = "JobApplicant"}
            };
        }
    }
}