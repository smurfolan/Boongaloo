using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boongaloo.Repository.Entities
{
    public class AreaToGroup
    {
        public int Id { get; set; }

        public int AreaId { get; set; }
        public int GroupId { get; set; }
    }
}
