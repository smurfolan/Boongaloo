using System;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.Linq;
using System.Text;
using Boongaloo.DTO;
using IdentityModel;
using Newtonsoft.Json.Linq;

namespace Boongaloo.MVCClient.Helpers
{
    // code adjusted from Thinktecture's client model (thinktecture.github.com)
    public static class TokenHelper
    {
        public static void DecodeAndWrite(string token)
        {
            // Write to output
            Debug.Write(ParseJsonFromToken(token));
        }

        public static UserInfoFromAccessToken GetUserInfoFromAccessToken(string token)
        {
            var deserializedInfo = ParseJsonFromToken(token);
            var jsonNetObject = JObject.Parse(deserializedInfo);

            return new UserInfoFromAccessToken()
            {
                FirstName = jsonNetObject.GetValue(JwtClaimTypes.GivenName).Value<string>(),
                LastName = jsonNetObject.GetValue(JwtClaimTypes.FamilyName).Value<string>(),
                Email = jsonNetObject.GetValue(JwtClaimTypes.Email).Value<string>()
            };
        }

        private static string ParseJsonFromToken(string token)
        {
            try
            {
                var parts = token.Split('.');

                string partToConvert = parts[1];
                partToConvert = partToConvert.Replace('-', '+');
                partToConvert = partToConvert.Replace('_', '/');
                switch (partToConvert.Length % 4)
                {
                    case 0:
                        break;
                    case 2:
                        partToConvert += "==";
                        break;
                    case 3:
                        partToConvert += "=";
                        break;
                    default:
                        break;
                }

                var partAsBytes = Convert.FromBase64String(partToConvert);
                var partAsUTF8String = Encoding.UTF8.GetString(partAsBytes, 0, partAsBytes.Count());

                // Json .NET
                var jwt = JObject.Parse(partAsUTF8String);

                // Write to output
                return jwt.ToString();
            }
            catch (Exception ex)
            {
                // something went wrong
                Debug.Write(ex.Message);
                return string.Empty;
            }
        }
    }
}

