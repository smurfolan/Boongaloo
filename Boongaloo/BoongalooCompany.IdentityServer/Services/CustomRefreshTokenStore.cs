using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using RefreshToken = IdentityServer3.Core.Models.RefreshToken;

namespace BoongalooCompany.IdentityServer.Services
{
    public class CustomRefreshTokenStore : IRefreshTokenStore
    {
        public Task StoreAsync(string key, RefreshToken value)
        {
            using (var context = new RefreshTokenRepository())
            {
                context.Insert(key, value);
            }

            return Task.FromResult(0);
        }

        public Task<RefreshToken> GetAsync(string key)
        {
            using (var context = new RefreshTokenRepository())
            {
                return Task.FromResult(context.Get(key));
            }
        }

        public Task RemoveAsync(string key)
        {
            using (var context = new RefreshTokenRepository())
            {
                context.Remove(key);
            }

            return Task.FromResult(0);
        }

        public Task<IEnumerable<ITokenMetadata>> GetAllAsync(string subject)
        {
            using (var context = new RefreshTokenRepository())
            {
                var result =
                    (IEnumerable<ITokenMetadata>)context.All(subject)
                        .Select(a => a.Token)
                        .Select(
                            n => new TokenMetadata(n.SubjectId, n.ClientId, n.Scopes));

                return Task.FromResult(result);
            }
        }

        public Task RevokeAsync(string subject, string client)
        {
            throw new NotImplementedException();
        }
    }
}