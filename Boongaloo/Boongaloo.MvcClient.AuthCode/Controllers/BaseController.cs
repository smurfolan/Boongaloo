using Boongaloo.MvcClient.AuthCode.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class BaseController : Controller
    {
        private readonly string tokensKey = "Tokens";

        private static Dictionary<string, object> CommonStorage 
            = new Dictionary<string, object>();

        public BaseController() { }
        
        protected TokenModel Tokens {
            get
            {
                return CommonStorage[tokensKey] as TokenModel;
            }
            set
            {
                if (value is TokenModel)
                {
                    CommonStorage[tokensKey] = value;
                }
            }
        }
    }
}