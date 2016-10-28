using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel
{
    [Table("MyEntities")]
    public class MyEntity
    {
        [Key]
        public int Id { get; set; }

        public string MyColumn { get; set; }
    }
}
