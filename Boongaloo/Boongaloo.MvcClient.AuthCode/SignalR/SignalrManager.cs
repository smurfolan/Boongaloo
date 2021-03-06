﻿using Boongaloo.MvcClient.AuthCode.SignalR.DTOs;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Boongaloo.MvcClient.AuthCode.SignalR
{
    public static class SignalrManager
    {
        /*Connect as Kevin to the SignalR hub*/
        private static string UserId = "5b8e69b6-fc13-494d-9228-4215de85254f";

        public static async Task ConnectToSignalRAsync()
        {
            var hubConnection = new HubConnection(Constants.BoongalooStagingAPI, new Dictionary<string, string>()
            {
                { "userId", UserId }
            });

            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("BoongalooGroupsActivityHub");

            stockTickerHubProxy
                .On<IEnumerable<Guid>, SRGroupDto, bool>(
                    "GroupAttachedToExistingAreasWasCreatedAroundMe",
                    GroupAttachedToExistingAreasWasCreatedAroundMe);

            stockTickerHubProxy
                .On<SRAreaDto, SRGroupDto, bool>(
                    "GroupAsNewAreaWasCreatedAroundMe",
                    GroupAsNewAreaWasCreatedAroundMe);

            stockTickerHubProxy
                .On<IEnumerable<SRAreaDto>, SRGroupDto, bool>(
                    "GroupAroundMeWasRecreated",
                    GroupAroundMeWasRecreated);

            stockTickerHubProxy
                .On<Guid>(
                    "GroupWasJoinedByUser",
                    GroupWasJoinedByUser);

            stockTickerHubProxy
                .On<Guid>(
                    "GroupWasLeftByUser",
                    GroupWasLeftByUser);

            await hubConnection.Start();
        }

        private static void GroupWasLeftByUser(Guid groupId)
        {
            Debug.WriteLine($"Group id:{groupId}.");
        }

        private static void GroupWasJoinedByUser(Guid groupId)
        {
            Debug.WriteLine($"Group id:{groupId}.");
        }

        private static void GroupAroundMeWasRecreated(
            IEnumerable<SRAreaDto> areaDtos, 
            SRGroupDto groupDto, 
            bool isSubscribedByMe)
        {
            Debug.WriteLine($"AreaDtos:{areaDtos}, GroupDto:{groupDto}, IsSubsribedByMe:{isSubscribedByMe}.");
        }

        public static void GroupAsNewAreaWasCreatedAroundMe(
            SRAreaDto areaDto, 
            SRGroupDto groupDto, 
            bool isSubscribedByMe)
        {
            Debug.WriteLine($"AreaDto:{areaDto}, GroupDto:{groupDto}, IsSubsribedByMe:{isSubscribedByMe}.");
        }

        public static void GroupAttachedToExistingAreasWasCreatedAroundMe(
            IEnumerable<Guid> areaIds,
            SRGroupDto groupDto,
            bool isSubsribedByMe)
        {
            Debug.WriteLine($"AreaIds:{areaIds.ToString()}, GroupDto:{groupDto.ToString()}, IsSubsribedByMe:{isSubsribedByMe}.");
        }
    }
}