using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace BoongalooCompany.Repository.Entities
{
    public class TokenMetadata : ITokenMetadata
    {
        public string SubjectId { get; }
        public string ClientId { get; }
        public IEnumerable<string> Scopes { get; }

        public TokenMetadata(string subjId, string clientId, IEnumerable<string> scopes)
        {
            this.SubjectId = subjId;
            this.ClientId = clientId;
            this.Scopes = scopes;
        }
    }
}
