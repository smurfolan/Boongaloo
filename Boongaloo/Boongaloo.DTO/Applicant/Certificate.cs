namespace Boongaloo.DTO.Applicant
{
    public class Certificate
    {
        // If we parse the user profile from LinkedIn, we could store the Id of the position, otherwise it is empty.
        public int LinkedInIdentifier { get; set; }

        public string Name { get; set; }
    }
}
