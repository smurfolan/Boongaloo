using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;

namespace Boongaloo.MVCClient.Helpers
{
    public static class ClaimsTransformations
    {
        /// <summary>
        /// Once we are authenticated from the IdentityServer, we need to assign claims to the identity of the logged in user.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="accessToken"></param>
        /// <returns>Assigns claims to the identity of the user. They could be all of the claims that came from the STS or filtered ones.</returns>
        public static async Task AssignClaimsReceivedFromIdentityServer(
            SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> n, 
            string accessToken)
        {
            var claimsFromAccessToken = TokenHelper.GetUserInfoFromAccessToken(accessToken);

            // FIRST NAME
            var givenNameClaim = new Claim(IdentityModel.JwtClaimTypes.GivenName, claimsFromAccessToken.FirstName);
            // n.AuthenticationTicket.Identity.FindFirst(IdentityModel.JwtClaimTypes.GivenName);

            // LAST NAME
            var familyNameClaim = new Claim(IdentityModel.JwtClaimTypes.FamilyName, claimsFromAccessToken.LastName);
            // n.AuthenticationTicket.Identity.FindFirst(IdentityModel.JwtClaimTypes.FamilyName);

            // ROLE
            var roleClaim = new Claim(IdentityModel.JwtClaimTypes.Role, claimsFromAccessToken.Role.ToString());
            // n.AuthenticationTicket.Identity.FindFirst(IdentityModel.JwtClaimTypes.Role);

            // SUBJECT
            var subClaim = n.AuthenticationTicket.Identity.FindFirst(IdentityModel.JwtClaimTypes.Subject);

            // EMAIL
            var emailClaim = new Claim(IdentityModel.JwtClaimTypes.Email, claimsFromAccessToken.Email); 

            // PHONE
            var phoneNumberClaim = new Claim(IdentityModel.JwtClaimTypes.PhoneNumber, claimsFromAccessToken.PhoneNumber);
            
            // SKYPE
            var skypeNameClaim = new Claim("skypename", claimsFromAccessToken.SkypeName);

            // create a new claims, issuer + sub as unique identifier
            var nameClaim = new Claim(IdentityModel.JwtClaimTypes.Name, Constants.BoongalooIssuerUri + subClaim.Value);

            var newClaimsIdentity = new ClaimsIdentity(
                n.AuthenticationTicket.Identity.AuthenticationType,
                IdentityModel.JwtClaimTypes.Name,
                IdentityModel.JwtClaimTypes.Role);

            newClaimsIdentity.AddClaim(nameClaim);
            newClaimsIdentity.AddClaim(givenNameClaim);
            newClaimsIdentity.AddClaim(familyNameClaim);          
            newClaimsIdentity.AddClaim(roleClaim);
            newClaimsIdentity.AddClaim(emailClaim);
            newClaimsIdentity.AddClaim(phoneNumberClaim);
            newClaimsIdentity.AddClaim(skypeNameClaim);

            // request a refresh token
            var tokenClientForRefreshToekn = new TokenClient(
                Constants.BoongalooSTSTokenEndpoint,
                "boongaloohybrid",
                Constants.BoongalooClientSecret);

            var refreshResponse = await tokenClientForRefreshToekn
                .RequestAuthorizationCodeAsync(
                    n.ProtocolMessage.Code,
                    Constants.BoongalooMVC);

            var expirationDateAsRoundtripString = DateTime
                .SpecifyKind(DateTime.UtcNow.AddSeconds(refreshResponse.ExpiresIn)
                    , DateTimeKind.Utc).ToString("o");

            newClaimsIdentity.AddClaim(new Claim("refresh_token", refreshResponse.RefreshToken));
            newClaimsIdentity.AddClaim(new Claim("access_token", refreshResponse.AccessToken));
            newClaimsIdentity.AddClaim(new Claim("expires_at", expirationDateAsRoundtripString));
            newClaimsIdentity.AddClaim(new Claim("id_token", refreshResponse.IdentityToken));

            // create a new authentication ticket, overwriting the old one.
            n.AuthenticationTicket = new AuthenticationTicket(
                newClaimsIdentity,
                n.AuthenticationTicket.Properties);
        }
    }
}