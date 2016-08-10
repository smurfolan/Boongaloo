﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Boongaloo.MVCClient.Helpers;
using Boongaloo.MVCClient.Models;
using IdentityModel.Client;
using Marvin.JsonPatch;
using Newtonsoft.Json;
using Boongaloo;
using Boongaloo.DTO;

namespace Boongaloo.MVCClient.Controllers
{
    [Authorize]
    public class TripsController : Controller
    {
        // GET: Trips
        public async Task<ActionResult> Index()
       {
            if (this.User.Identity.IsAuthenticated)
            {
                var identity = this.User.Identity as ClaimsIdentity;
                foreach (var claim in identity.Claims)
	            {
                    Debug.WriteLine(claim.Type + " - " + claim.Value);
	            }               
            } 

            var httpClient = BoongalooHttpClient.GetClient();

            var rspTrips = await httpClient.GetAsync("api/trips").ConfigureAwait(false);

            if (rspTrips.IsSuccessStatusCode)
            {
                var lstTripsAsString = await rspTrips.Content.ReadAsStringAsync().ConfigureAwait(false);          

                var vm = new TripsIndexViewModel();
                vm.Trips = JsonConvert.DeserializeObject<IList<Trip>>(lstTripsAsString).ToList();

                return View(vm);
            }
            else
            {
                return View("Error",
                         new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(rspTrips),
                        "Trips", "Index"));              
            }        
        }

        // GET: Trips/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
         


        // GET: Trips/Create
        public ActionResult Create()
        {
            return View(new TripCreateViewModel(new TripForCreation()));
        }


        public async Task<ActionResult> Album(Guid tripId)
        {
            // get the access token.
            var token = (User.Identity as ClaimsIdentity).FindFirst("access_token").Value;

             UserInfoClient userInfoClient = new UserInfoClient(
                    new Uri(Constants.BoongalooSTSUserInfoEndpoint),
                    token);

            var userInfoResponse = await userInfoClient.GetAsync();

            if (!userInfoResponse.IsError)
            {
                // create an object to return (dynamic Expando - anonymous 
                // types won't allow access to their properties from the view)
                dynamic addressInfo = new ExpandoObject();
                addressInfo.Address = userInfoResponse.Claims.First(c => c.Item1 == "address").Item2; 

                return View(addressInfo);
            }
            else
            {          
                var exception = new Exception("Problem getting your address.  Please contact your administrator.");
                return View("Error", new HandleErrorInfo(exception, "Trips", "Album"));    
            }             
        }

        // POST: Trips/Create
        [HttpPost]
        public async Task<ActionResult> Create(TripCreateViewModel vm)
        {
            try
            {
 
                byte[] uploadedImage = new byte[vm.MainImage.InputStream.Length];
                vm.MainImage.InputStream.Read(uploadedImage, 0, uploadedImage.Length);

                vm.Trip.MainPictureBytes = uploadedImage;

                var httpClient = BoongalooHttpClient.GetClient();

                var serializedTrip = JsonConvert.SerializeObject(vm.Trip);

                var response = await httpClient.PostAsync("api/trips", 
                    new StringContent(serializedTrip, System.Text.Encoding.Unicode, "application/json")).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Trips");                        
                }
                else
                {
                    return View("Error", 
                            new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(response), 
                            "Trips", "Create"));    
                } 
            }
            catch  (Exception ex)
            {  
                return View("Error", new HandleErrorInfo(ex, "Trips", "Create"));    
            }
        }

     
        public async Task<ActionResult> SwitchPrivacyLevel(Guid id, bool isPublic)
        {
            
            // create a patchdocument to change the privacy level of this trip

            JsonPatchDocument<Trip> tripPatchDoc = new JsonPatchDocument<Trip>();
            tripPatchDoc.Replace(t => t.IsPublic, !isPublic);

            var httpClient = BoongalooHttpClient.GetClient();

            var rspPatchTrip = await httpClient.PatchAsync("api/trips/" + id.ToString(),
                 new StringContent(JsonConvert.SerializeObject(tripPatchDoc), System.Text.Encoding.Unicode, "application/json"))
                 .ConfigureAwait(false);

            if (rspPatchTrip.IsSuccessStatusCode)
            {
                // the patch was succesful.  Reload.
                return RedirectToAction("Index", "Trips");                          
            }
            else
            {
                return View("Error",
                         new HandleErrorInfo(ExceptionHelper.GetExceptionFromResponse(rspPatchTrip),
                        "Trips", "SwitchPrivacyLevel"));              
            }
        } 
    }
}
