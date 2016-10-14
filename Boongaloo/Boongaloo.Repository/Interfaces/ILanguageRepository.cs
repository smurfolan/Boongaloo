using Boongaloo.Repository.Entities;
using System.Collections.Generic;

namespace Boongaloo.Repository.Interfaces
{
    public interface ILanguageRepository
    {
        IEnumerable<Language> GetLangauges();
    }
}
