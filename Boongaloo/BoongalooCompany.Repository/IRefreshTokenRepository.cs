using System.Collections;
using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace BoongalooCompany.Repository
{
    public interface IRefreshTokenRepository
    {
        void Insert(string key, RefreshToken token);
        RefreshToken Get(string key);
        void Remove(string key);
        IList<Entities.RefreshToken> All(string subject);
    }
}
