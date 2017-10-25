﻿using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoongalooCompany.Repository;
using BoongalooCompany.Repository.Entities;
using IdentityServer3.Core.Extensions;
using System.Security.Claims;
using BoongalooCompany.IdentityServer.Helpers;

namespace BoongalooCompany.IdentityServer.Services
{
    public class CustomUserService : UserServiceBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Authenticate against local user store.
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            using (var userRepository = new UserRepository())
            {
                // get user from repository
                var user = userRepository.GetUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.AuthenticateResult = new AuthenticateResult("Invalid credentials");
                    return Task.FromResult(0);
                }

                context.AuthenticateResult = new AuthenticateResult(
                    user.Subject,
                    user.UserClaims.First(c => c.ClaimType == Constants.ClaimTypes.GivenName).ClaimValue);

                return Task.FromResult(0);
            }
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            using (var userRepository = new UserRepository())
            {
                // find the user
                var user = userRepository.GetUser(context.Subject.GetSubjectId());

                // add subject as claim
                var claims = new List<Claim>
                {
                    new Claim(Constants.ClaimTypes.Subject, user.Subject),
                };

                // add the other UserClaims
                claims.AddRange(user.UserClaims.Where(uc => uc.ClaimValue != null).Select<UserClaim, Claim>(
                    uc => new Claim(uc.ClaimType, uc.ClaimValue)));

                // only return the requested claims
                if (!context.AllClaimsRequested)
                {
                    claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                }

                // set the issued claims - these are the ones that were requested, if available
                context.IssuedClaims = claims;

                return Task.FromResult(0);
            }
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            using (var userRepository = new UserRepository())
            {
                if (context.Subject == null)
                {
                    throw new ArgumentNullException("subject");
                }

                var user = userRepository.GetUser(context.Subject.GetSubjectId());

                // set whether or not the user is active
                context.IsActive = (user != null) && user.IsActive;

                return Task.FromResult(0);
            }
        }

        // gets called whenever the user uses external identity provider to authenticate
        // now we will try to map external user to a local user
        public override Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            using (var userRepository = new UserRepository())
            {
                Log.Info("Inside AuthenticateExternalAsync");
                // is the external provider already linked to an account?
                var existingLinkedUser = userRepository.GetUserForExternalProvider(context.ExternalIdentity.Provider,
                 context.ExternalIdentity.ProviderId);

                // it is - set as authentication result;
                if (existingLinkedUser != null)
                {
                    Log.Info("User for external provider exists in the DB.");
                    context.AuthenticateResult = new AuthenticateResult(
                        existingLinkedUser.Subject,
                        existingLinkedUser.UserClaims.First(c => c.ClaimType == Constants.ClaimTypes.GivenName).ClaimValue,
                        existingLinkedUser.UserClaims.Select<UserClaim, Claim>(uc => new Claim(uc.ClaimType, uc.ClaimValue)),
                        authenticationMethod: Constants.AuthenticationMethods.External,
                        identityProvider: context.ExternalIdentity.Provider);

                    Log.Info("Authentication result was added to the context.");
                    return Task.FromResult(0);
                }

                Log.Info("User for external provider does not exist in the DB.");
                // no existing link, get email claim to match user
                var emailClaim = context.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "email");
                if (emailClaim == null)
                {
                    Log.Info("No user claim available from the external provider.");
                    // return error - we need an email claim to match
                    context.AuthenticateResult = new AuthenticateResult("No email claim available.");
                    return Task.FromResult(0);
                }
                Log.Info("There's email claim in the result from the external provider.");
                // find a user with a matching e-mail claim.  
                var userWithMatchingEmailClaim = userRepository.GetUserByEmail(emailClaim.Value);

                if (userWithMatchingEmailClaim == null && 
                    (context.ExternalIdentity.Provider.ToLower() == "google"))
                {
                    Log.Info("There's no user with matching email in the DB and provider is google.");
                    var newUser = new User
                    {
                        Subject = Guid.NewGuid().ToString(),
                        IsActive = true
                    };

                    newUser.UserLogins.Add(new UserLogin()
                    {
                        Subject = newUser.Subject,
                        LoginProvider = context.ExternalIdentity.Provider,
                        ProviderKey = context.ExternalIdentity.Provider
                    });

                    ClaimsHelper.AddUserClaimsForExternalUser(null, newUser, context.ExternalIdentity.Claims);

                    userRepository.AddUser(newUser);
                    Log.Info("New user was created and added to the DB.");
                }

                userWithMatchingEmailClaim = userRepository.GetUserByEmail(emailClaim.Value);

                // register this external provider for this user account
                userRepository.AddUserLogin(userWithMatchingEmailClaim.Subject,
                     context.ExternalIdentity.Provider,
                     context.ExternalIdentity.ProviderId);

                 // use this existing account
                 context.AuthenticateResult = new AuthenticateResult(
                     userWithMatchingEmailClaim.Subject,
                     userWithMatchingEmailClaim.UserClaims.First(c => c.ClaimType == Constants.ClaimTypes.GivenName).ClaimValue,
                     userWithMatchingEmailClaim.UserClaims.Select<UserClaim, Claim>(uc => new Claim(uc.ClaimType, uc.ClaimValue)),
                     authenticationMethod: Constants.AuthenticationMethods.External,
                     identityProvider: context.ExternalIdentity.Provider);

                Log.Info("External provider was assigned to the newly created user and AuthenticationResult was set to the context with that new user.");

            }
            
            return Task.FromResult(0);
        }
    }
}
