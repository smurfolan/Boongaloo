using System.Collections.Generic;
using Boongaloo.DTO;

namespace Boongaloo.DTO.Applicant
{
    public class UserData : UserInfoFromAccessToken 
    {
        public string UniqueId
        {
            get
            {
                return string.Format("{0}{1}", this.IdentityProvider, this.Issuer);
            }
        }

        public string Issuer { get; set; }
        public string IdentityProvider { get; set; }
        public List<Skill> Skills { get; set; }
        public List<JobPosition> Experiences { get; set; }
        public List<Education> Educations { get; set; }
        public List<Certificate> Certificates { get; set; }
    }
}
