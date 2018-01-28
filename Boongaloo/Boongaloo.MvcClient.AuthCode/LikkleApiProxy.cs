using Boongaloo.MvcClient.AuthCode.Models;
using RestSharp;
using System.Collections.Generic;

namespace Boongaloo.MvcClient.AuthCode
{
    public class LikkleApiProxy
    {
        private RestClient likkleApiClient;
        private TokenModel tokenModel;

        public LikkleApiProxy(TokenModel tokenModel)
        {
            this.likkleApiClient = new RestClient(Constants.BoongalooStagingAPI);
            this.tokenModel = tokenModel;
        }

        public T Get<T>(
            string url, 
            Dictionary<string, string> headers = null, 
            Dictionary<string, string> queryParams = null) where T : new() 
        {
            var request = this.BuildRootUrl(url, Method.GET);
            
            if(headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            if(queryParams != null)
            {
                foreach (var queryParam in queryParams)
                {
                    request.AddParameter(queryParam.Key, queryParam.Value);
                }
            }

            var response = likkleApiClient.Execute<T>(request);

            return response.Data;
        }

        private RestRequest BuildRootUrl(string baseUrl, Method method)
        {
            var request = new RestRequest($"api/v1/{baseUrl}", method);
            request.AddHeader("Authorization", $"Bearer {tokenModel.AccessToken}");

            return request;
        }
    }
}