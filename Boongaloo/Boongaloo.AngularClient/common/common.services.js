(function () {
    "use strict";

    angular.module("common.services",
                    ["ngResource"])
      	.constant("appSettings",
        {
            tripGalleryAPI: "https://localhost:44315" 
        });


    // oidc manager for dep injection
    angular.module("common.services")
        .factory("OidcManager", function () {

            // configure manager
            var config = {
                client_id: "tripgalleryimplicit",
                redirect_uri:  window.location.protocol + "//" + window.location.host + "/callback.html",
                response_type: "id_token token",
                scope: "openid profile address gallerymanagement roles",               
                authority: "https://localhost:44317/identity",
                // This one has to match what is defined on identity server level
                post_logout_redirect_uri: window.location.protocol + "//" + window.location.host + "/index.html",
                silent_redirect_uri: window.location.protocol + "//" + window.location.host + "/silentrefreshframe.html",
                silent_renew: true /*oidc manager checks periodically for the expiration of the access token*/
            };
                    
            var mgr = new OidcTokenManager(config);
             
            return {
                OidcTokenManager: function () {
                    return mgr;
                } 
        };
    });


}());
