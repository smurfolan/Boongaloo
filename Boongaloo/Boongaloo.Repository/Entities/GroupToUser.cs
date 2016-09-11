using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boongaloo.Repository.Entities
{
    public class GroupToUser
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
