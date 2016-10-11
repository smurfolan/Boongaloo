using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetTags();

        void Save();
    }
}
