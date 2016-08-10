namespace Boongaloo.DTO.Applicant
{
    public class Company
    {
        // If we parse the user profile from LinkedIn, we could store the Id of the position, otherwise it is empty.
        public int LinkedInIdentifier { get; set; }

        public string Industry { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        // TODO: Extract enumeration
        public string Type { get; set; }
    }
}
