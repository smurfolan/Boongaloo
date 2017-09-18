using Boongaloo.MvcClient.AuthCode.SignalR.DTOs;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Boongaloo.MvcClient.AuthCode.SignalR
{
    public static class SignalrManager
    {
        public static async Task ConnectToSignalRAsync()
        {
            var hubConnection = new HubConnection("http://localhost:54036/", new Dictionary<string, string>()
            {
                { "userId", "52360a79-7f57-4a70-9590-c632196f8a56" }
            });

            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("BoongalooGroupsActivityHub");

            stockTickerHubProxy
                .On<IEnumerable<Guid>, SRGroupDto, bool>(
                "GroupAttachedToExistingAreasWasCreatedAroundMe",
                (areaIds, groupDto, isSubsribedByMe) => GroupAttachedToExistingAreasWasCreatedAroundMe(areaIds, groupDto, isSubsribedByMe));

            await hubConnection.Start();
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