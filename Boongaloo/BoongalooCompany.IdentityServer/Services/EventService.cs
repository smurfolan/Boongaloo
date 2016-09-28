using System.Threading.Tasks;
using IdentityServer3.Core.Events;
using IdentityServer3.Core.Services;
using Serilog;

namespace BoongalooCompany.IdentityServer.Services
{
    public class EventService : IEventService
    {
        static readonly ILogger Log;

        static EventService()
        {
            Log = new LoggerConfiguration()
                .WriteTo.File("BoongalooIdsrvLogs-{Date}.txt")
                .CreateLogger();
        }

        public Task RaiseAsync<T>(Event<T> evt)
        {
            Log.Information("{Id}: {Name} / {Category} ({EventType}), Context: {@context}, Details: {@details}",
                evt.Id,
                evt.Name,
                evt.Category,
                evt.EventType,
                evt.Context,
                evt.Details);

            return Task.FromResult(0);
        }
    }
}