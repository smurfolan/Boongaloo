using Boongaloo.MvcClient.AuthCode.Models;
using System.Web.Mvc;

namespace Boongaloo.MvcClient.AuthCode.Controllers
{
    public class BaseController : Controller
    {
        private readonly string tokensKey = "Tokens";

        protected TokenModel Tokens {
            get
            {
                return Session[tokensKey] as TokenModel;
            }
            set
            {
                if (value is TokenModel)
                {
                    Session[tokensKey] = value;
                }
            }
        }
    }
}