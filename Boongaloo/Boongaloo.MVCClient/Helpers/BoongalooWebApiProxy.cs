using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Boongaloo.DTO.Applicant;
using System.Net.Http.Formatting;
using Boongaloo.DTO.BoongalooWebApiDto;

namespace Boongaloo.MVCClient.Helpers
{
    // TODO: See how to ensure API result from HttpResponseMessage
    public class BoongalooWebApiProxy
    {
        private HttpClient _client;

        public BoongalooWebApiProxy()
        {
            this._client = BoongalooHttpClient.GetClient();
        }

        public async Task<UserAddedResponse> AddNewUser(UserData newUser)
        {
            // TODO: Consider using SecurePostAsJsonAsync
            var result = await this._client.PostAsJsonAsync("api/UserData/AddNewUser", newUser);

            return await EnsureApiResult<UserAddedResponse>(result);
        }

        public async Task<UserData> GetUserBySubjectAsync(string subject)
        {
            var userDataResponse = await this._client
                .GetAsync("api/UserData/GetUserBySubject?userSubject=" + subject);

            return await EnsureApiResult<UserData>(userDataResponse);
        }

        protected async Task<T> EnsureApiResult<T>(HttpResponseMessage result)
        {
            if (result.StatusCode != HttpStatusCode.OK
                && result.StatusCode != HttpStatusCode.Created)
                ExceptionalScenarioHandler<T>(result);

            return await result.Content.ReadAsAsync<T>();
        }

        private static void ExceptionalScenarioHandler<T>(HttpResponseMessage result)
        {
            // TODO: Add proper handling
            //if (result.StatusCode == HttpStatusCode.NotFound)
            //{
            //    var apiException =
            //        new MemberApiException
            //        {
            //            ErrorMessage = "Method not found"
            //        };
            //    throw new MemberApiProxyException(apiException, result.StatusCode);
            //}
            //else if (result.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //    var details = result.Content.ReadAsAsync<UnauthorizedDetails>().Result;
            //    MemberApiException apiException;
            //    try
            //    {
            //        apiException = JsonConvert.DeserializeObject<MemberApiException>(details.error_description);
            //    }
            //    catch
            //    {
            //        apiException = new MemberApiException
            //        {
            //            ErrorMessage = HttpStatusCode.Unauthorized.ToString()
            //        };
            //    }
            //    throw new MemberApiProxyException(apiException, result.StatusCode);
            //}
            //else if (result.StatusCode == HttpStatusCode.BadRequest)
            //{
            //    var error = result.Content.ReadAsAsync<HttpError>().Result;

            //    if (!error.HasValidationErrors())
            //    {
            //        throw new Exceptions.MemberApiException(error.Message, result.StatusCode);
            //    }

            //    var validationErrors = error.GetValidationErrors();

            //    throw new MemberApiValidationException(validationErrors);
            //}
            //else
            //{
            //    var apiException = result.Content.ReadAsAsync<MemberApiException>().Result;
            //    throw new MemberApiProxyException(apiException, result.StatusCode);
            //}
        }
    }
}