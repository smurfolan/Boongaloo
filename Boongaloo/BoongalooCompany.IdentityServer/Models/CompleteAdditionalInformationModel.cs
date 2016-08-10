using System.Collections.Generic;
using System.Web.Mvc;

namespace BoongalooCompany.IdentityServer.Models
{
    // If the user uses external login, this is the model we are using in order to collect additional information from him before getting him logged in.
    public class CompleteAdditionalInformationModel : BaseUserInfo
    {
        public string ExternalProvider { get; set; }
    }
}
