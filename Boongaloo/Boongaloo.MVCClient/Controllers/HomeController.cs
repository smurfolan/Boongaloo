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
        public async Task<ActionResult> PostAuthorization()
        {
            // Need to determine if the user is available or not in our system. If yes, redirect to a separate view, else go to the bottom one.
            // That is the place where he has to add additional information and get stored into the DB.

            var userId = ClaimsPrincipal.Current
                .Claims
                .First(claim => claim.Type == IdentityModel.JwtClaimTypes.Name).Value;

            var boongalooWebApiProxy = new BoongalooWebApiProxy();

            var userFromStorage = await boongalooWebApiProxy.GetUserBySubjectAsync(userId);

            if (userFromStorage != null)
            {
                // Navigate to a separate view for a use with completed profile
            }

            var userRole = ClaimsPrincipal.Current.Claims.First(claim => claim.Type == IdentityModel.JwtClaimTypes.Role).Value;

            if(userRole == RolesEnum.JobApplicant.ToString())
                return RedirectToAction("Index", "ApplicantRelatedInfo", new { area = "Application" });

            else if(userRole == RolesEnum.Recruiter.ToString())
                return RedirectToAction("Index", "RecruiterRelatedInfo", new { area = "Recruitment" });

            return null;
        }
    }
}