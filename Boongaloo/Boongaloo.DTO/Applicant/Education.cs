using System;

namespace Boongaloo.DTO.Applicant
{
    public class Education
    {
        // If we parse the user profile from LinkedIn, we could store the Id of the position, otherwise it is empty.
        public int LinkedInIdentifier { get; set; }

        public string Activities { get; set; }
        // TODO: Extract enumeration
        public string Degree { get; set; }
        public DateTime StarDate { get; set; }
        public DateTime EndDate { get; set; }
        // TODO: Extract enumeration
        public string FieldOfStudy { get; set; }
        public string Notes { get; set; }
        public string SchoolName { get; set; }

    }
}
