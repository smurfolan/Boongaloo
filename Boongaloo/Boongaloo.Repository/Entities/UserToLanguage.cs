namespace Boongaloo.Repository.Entities
{
    public class UserToLanguage
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }
        public int UserId { get; set; }
    }
}
