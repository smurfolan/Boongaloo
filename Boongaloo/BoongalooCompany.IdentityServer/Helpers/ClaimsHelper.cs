using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BoongalooCompany.IdentityServer.Models;
using BoongalooCompany.Repository.Entities;

namespace BoongalooCompany.IdentityServer.Helpers
{
    public static class ClaimsHelper
    {
        /// <summary>
        /// Collects all the user provided information into list of claims related to the user in our store
        /// </summary>
        /// <param name="newUser">The user that logged in from an external account</param>
        /// <param name="model">User provided information</param>
        /// <param name="claims">Claims for the The user who is currently partially logged in.</param>
        public static void AddUserClaimsForExternalUser(
            CompleteAdditionalInformationModel model,
            User newUser,
            IEnumerable<Claim> claims)
        {
            var claimsArray = claims as Claim[] ?? claims.ToArray();

            // EMAIL
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.Email,
                ClaimValue = claimsArray.First(
                    c => c.Type == IdentityServer3.Core.Constants.ClaimTypes.Email).Value
            });

            // GIVEN NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.GivenName,
                ClaimValue = claimsArray.First(c => c.Type == "given_name").Value
            });

            // FAMILY NAME
            newUser.UserClaims.Add(new UserClaim()
            {
                Id = Guid.NewGuid().ToString(),
                Subject = newUser.Subject,
                ClaimType = IdentityServer3.Core.Constants.ClaimTypes.FamilyName,
                ClaimValue = claimsArray.First(c => c.Type == "family_name").Value
            });
        }
    }
}