using System.Web;
using System.Web.Mvc;

namespace Boongaloo.MVCClient.Controllers
{
    public class UserAccountController : Controller
    {
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        // End-point used for the single sign-out option.
        public ActionResult LocalLogout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}