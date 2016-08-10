using System;

namespace Boongaloo.DTO.Applicant
{
    public class JobPosition
    {
        // If we parse the user profile from LinkedIn, we could store the Id of the position, otherwise it is empty.
        public int LinkedInIdentifier { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public Company Company { get; set; }
        public string Location { get; set; }
    }
}
