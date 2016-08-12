using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Boongaloo.DTO;
using Boongaloo.MVCClient.Helpers;

namespace Boongaloo.MVCClient.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            // this._boongalooWebApiProxy = new BoongalooWebApiProxy();
        }

        [Authorize]
        public ActionResult PostAuthorization()
        {
            // This is the point where the user was successfuly authorized by the STS
            var userId = ClaimsPrincipal.Current
                .Claims
                .First(claim => claim.Type == IdentityModel.JwtClaimTypes.Name).Value;
            
            return View(ClaimsPrincipal.Current.Claims);
        }
    }
}