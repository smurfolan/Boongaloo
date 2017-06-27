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
                this.RemovePreviousLoginsForCurrentUser(context, value.SubjectId);

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

        #region Private
        /// <summary>
        /// The purpose of this method is refresh token store optimization. After each login it grows with unneccessary data.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subjectId"></param>
        private void RemovePreviousLoginsForCurrentUser(RefreshTokenRepository context, string subjectId)
        {
            var previousLogin = context.All(subjectId);

            if (!previousLogin.Any())
                return;

            foreach (var item in previousLogin)
            {
                context.Remove(item.Key);
            }
        }
        #endregion
    }
}