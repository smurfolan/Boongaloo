using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Repositories
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetLanguages();

        void Save();
    }
}
