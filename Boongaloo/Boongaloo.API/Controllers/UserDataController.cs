using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Boongaloo.DTO.Applicant;
using Boongaloo.DTO.BoongalooWebApiDto;
using Boongaloo.Repository;

namespace Boongaloo.API.Controllers
{
    [Authorize]
    [EnableCors("https://localhost:44316", "*", "GET, POST, DELETE")]
    public class UserDataController : ApiController
    {
        private const string DataStoreFileLocation = "app_data/userinfostore.json";

        [HttpGet]
        public UserData GetUserBySubjectAsync(string userSubject)
        {
            if (userSubject != string.Empty)
            {
                using (var udr = new UserDataRepository(new UserDataContext(DataStoreFileLocation)))
                {
                    var searchedUser = udr.GetUser(userSubject);

                    return searchedUser ?? null;
                }
            }

            return null;
        }

        [HttpPost]
        public UserAddedResponse AddNewUser([FromBody]UserData newUser)
        {
            if (newUser != null)
            {
                using (var udr = new UserDataRepository(new UserDataContext(DataStoreFileLocation)))
                {
                    udr.AddUser(newUser);
                }
            }

            return new UserAddedResponse();
        }
    }
}
