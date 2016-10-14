using Boongaloo.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boongaloo.Repository.Interfaces
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags();
    }
}
