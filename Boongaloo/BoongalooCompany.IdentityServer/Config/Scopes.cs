using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boongaloo;

namespace BoongalooCompany.IdentityServer.Config
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
                { 
                    StandardScopes.OpenId,
                    StandardScopes.ProfileAlwaysInclude,
                    StandardScopes.Address,
                    // Needed when we are working with refresh tokens
                    StandardScopes.OfflineAccess,            
                    new Scope
                    { 
                        Name = "boongaloomanagement",
                        DisplayName = "Boongaloo Management",
                        Description = "Allow the application to manage boongaloo related entities on your behalf.",
                        Type = ScopeType.Resource,
                        // Once we ask for the boongaloo management scope, in the access token we receive,
                        // we will have a role claim.
                        Claims = new List<ScopeClaim>()
                        {
                            // All of these claims go into the access token.
                            new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Role, false),
                            new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.Email),
                            new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.PhoneNumber),
                            new ScopeClaim("skypename"),
                            new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.GivenName),
                            new ScopeClaim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName)
                        }
                    }
                };
        }
    }
}
