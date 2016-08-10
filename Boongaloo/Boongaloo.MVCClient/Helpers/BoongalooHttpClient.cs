﻿using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using IdentityModel.Client;
using Boongaloo;

namespace Boongaloo.MVCClient.Helpers
{
    public static class BoongalooHttpClient
    {

        public static HttpClient GetClient()
        { 
            HttpClient client = new HttpClient();

            // Since our API has 'app.UseIdentityServerBearerTokenAuthentication' option activated
            // we must assign bearer token to our web api request

            client.SetBearerToken(GetAccessToken());

            client.BaseAddress = new Uri(Constants.BoongalooAPI);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private static string GetAccessToken()
        {
            var currentClaimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            var expiresAtFromClaims = DateTime.Parse(currentClaimsIdentity.FindFirst("expires_at").Value,
                null,
                DateTimeStyles.RoundtripKind);

            // check if the access token hasn't expired
            if (DateTime.Now.ToUniversalTime() < expiresAtFromClaims)
            {
                return currentClaimsIdentity.FindFirst("access_token").Value;
            } 

            // Expired. Get a new one
            var tokenEndpointClient = new TokenClient(
                Constants.BoongalooSTSTokenEndpoint,
                "boongaloohybrid",
                Constants.BoongalooClientSecret);

            var requestRefreshTokenResponse =
                tokenEndpointClient.RequestRefreshTokenAsync(currentClaimsIdentity.FindFirst("refresh_token").Value)
                    .Result;

            if (!requestRefreshTokenResponse.IsError)
            {
                // replace the claims with the new values - this means creating a new identity
                var result = from claim in currentClaimsIdentity.Claims
                    where claim.Type != "access_token" && claim.Type != "refresh_token" &&
                          claim.Type != "expires_at" && claim.Type != "id_token"
                    select claim;

                var claims = result.ToList();

                var expirationDateAsRoundtripString =
                    DateTime.SpecifyKind(DateTime.UtcNow.AddSeconds(requestRefreshTokenResponse.ExpiresIn),
                        DateTimeKind.Utc).ToString("o");

                claims.Add(new Claim("access_token", requestRefreshTokenResponse.AccessToken));
                claims.Add(new Claim("expires_at", expirationDateAsRoundtripString));
                claims.Add(new Claim("refresh_token", requestRefreshTokenResponse.RefreshToken));

                // we'll have a new claims identity after the request has been completed,
                // containing the new tokens
                var newIdentity = new ClaimsIdentity(
                    claims,
                    "Cookies",
                    IdentityModel.JwtClaimTypes.Name,
                    IdentityModel.JwtClaimTypes.Role);

                HttpContext.Current.Request.GetOwinContext().Authentication.SignIn(newIdentity);

                // return the new access token
                return requestRefreshTokenResponse.AccessToken;
            }
            else
            {
                HttpContext.Current.Request.GetOwinContext().Authentication.SignOut();
                return "";
            }
        }
 
    }
}