using System;
using System.Collections.Generic;
using System.Linq;
using BoongalooCompany.Repository.Entities;
using RefreshToken = IdentityServer3.Core.Models.RefreshToken;

namespace BoongalooCompany.Repository
{
    public class RefreshTokenRepository : IDisposable, IRefreshTokenRepository
    {
        RefreshTokenContext _ctx;

        public RefreshTokenRepository(RefreshTokenContext userContext)
        {
            _ctx = userContext;
        }

        public RefreshTokenRepository()
        {
            // no context passed in, assume default location
            _ctx = new RefreshTokenContext(@"app_data/refreshtokenstore.json");
        }

        public void Dispose()
        {
            // do a code cleanup
        }

        public void Insert(string key, RefreshToken token)
        {
            this._ctx.RefreshTokens.Add(new Entities.RefreshToken()
            {
                Key = key,
                Token = token
            });

            this._ctx.SaveChanges();
        }

        public RefreshToken Get(string key)
        {
            var token = this._ctx.RefreshTokens.FirstOrDefault(t => t.Key == key);

            return token?.Token;
        }

        public void Remove(string key)
        {
            var toBeRemoved = this._ctx.RefreshTokens.FirstOrDefault(t => t.Key == key);

            this._ctx.RefreshTokens.Remove(toBeRemoved);

            this._ctx.SaveChanges();
        }

        public IList<Entities.RefreshToken> All(string subject)
        {
            return this._ctx.RefreshTokens.Where(rt => rt.Token.SubjectId == subject).ToList();
        }
    }
}
