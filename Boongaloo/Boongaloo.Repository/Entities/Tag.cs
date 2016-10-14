namespace Boongaloo.Repository.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Tag(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
