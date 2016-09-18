using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Boongaloo.DTO;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.MVCClient.Helpers;
using Newtonsoft.Json;

namespace Boongaloo.MVCClient.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        [System.Web.Mvc.Authorize]
        public ActionResult PostAuthorization()
        {
            // This is the point where the user was successfuly authorized by the STS
            var userId = ClaimsPrincipal.Current
                .Claims
                .First(claim => claim.Type == IdentityModel.JwtClaimTypes.Name).Value;
            
            return View(ClaimsPrincipal.Current.Claims);
        }

        public async Task<ActionResult> CreateNewUser(UserDto user)
        {
            var client = BoongalooHttpClient.GetClient();

            // serialize & POST
            var serializedItemToCreate = JsonConvert.SerializeObject(user);

            var response = await client.PostAsync("api/v1/users",
                    new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            else
            {
                return Content(response.ReasonPhrase);
            }            
        }

        public ActionResult UpdateUser(int id, EditUserDto editedUser)
        {
            return null;
        }

        public async Task<ActionResult> CreateNewGroup([FromBody]GroupDto group)
        {
            var client = BoongalooHttpClient.GetClient();

            // serialize & POST
            var serializedItemToCreate = JsonConvert.SerializeObject(group);

            var response = await client.PostAsync("api/v1/groups",
                    new StringContent(serializedItemToCreate,
                    System.Text.Encoding.Unicode, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            else
            {
                return Content(response.ReasonPhrase);
            }
        }

        #region Area
        public async Task<JsonResult> GetAllAreaIds(double lat, double lon)
        {
            var client = BoongalooHttpClient.GetClient();

            var response = await client.GetAsync($"api/v1/areas/{lat}/{lon}/");

            if (response.IsSuccessStatusCode)
            {
                var listOfAreasAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var listOfActualAreas = JsonConvert.DeserializeObject<IList<AreaDto>>(listOfAreasAsString).ToList();

                return Json(listOfActualAreas, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}