using Microsoft.AspNetCore.Components.Server.Circuits;

namespace ImageDiffFinder.Services
{
    /// <summary>
    /// Tutorials
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?preserve-view=true&view=aspnetcore-8.0#monitor-server-side-circuit-activity
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-8.0#access-server-side-blazor-services-from-a-different-di-scope
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/additional-scenarios?view=aspnetcore-8.0#access-authenticationstateprovider-in-outgoing-request-middleware
    /// https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-preview-3/
    /// 
    /// You can now monitor inbound circuit activity in Blazor Server apps using
    /// the new CreateInboundActivityHandler method on CircuitHandler.
    /// Inbound circuit activity is any activity sent from the browser to the server,
    /// like UI events or JavaScript-to-.NET interop calls.
    /// </summary>
    public class CircuitServicesAccessor
    {
        static readonly AsyncLocal<IServiceProvider> blazorServices = new();

        public IServiceProvider? Services
        {
            get => blazorServices.Value;
            set => blazorServices.Value = value;
        }
    }

    public class ServicesAccessorCircuitHandler : CircuitHandler
    {
        readonly IServiceProvider services;
        readonly CircuitServicesAccessor circuitServicesAccessor;

        public ServicesAccessorCircuitHandler(IServiceProvider services,
            CircuitServicesAccessor servicesAccessor)
        {
            this.services = services;
            this.circuitServicesAccessor = servicesAccessor;
        }

        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next)
        {
            return async context =>
            {
                circuitServicesAccessor.Services = services;
                await next(context);
                circuitServicesAccessor.Services = null;
            };
        }
    }

    //public static class CircuitServicesServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddCircuitServicesAccessor(
    //        this IServiceCollection services)
    //    {
    //        services.AddScoped<CircuitServicesAccessor>();
    //        services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

    //        return services;
    //    }
    //}
}
