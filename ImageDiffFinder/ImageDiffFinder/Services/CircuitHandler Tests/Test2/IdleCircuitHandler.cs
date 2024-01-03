using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;

namespace ImageDiffFinder.Services
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?preserve-view=true&view=aspnetcore-8.0#monitor-server-side-circuit-activity
    /// https://www.youtube.com/watch?v=MaLjiR9YSbs&t=3060s
    /// </summary>
    public sealed class IdleCircuitHandler : CircuitHandler, IDisposable
    {
        readonly System.Timers.Timer timer;
        readonly ILogger logger;

        public IdleCircuitHandler(/*IOptions<IdleCircuitOptions> options,*/
            ILogger<IdleCircuitHandler> logger)
        {
            timer = new System.Timers.Timer();
            //timer.Interval = options.Value.IdleTimeout.TotalMilliseconds;
            timer.Interval = TimeSpan.FromSeconds(5).TotalMilliseconds;
            timer.AutoReset = false;
            timer.Elapsed += IdleTimerElapsed;
            this.logger = logger;
        }

        private void IdleTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            logger.LogInformation("{Circuit} is idle", nameof(CircuitIdle));

            if(CircuitIdle is not null)
            {
                CircuitIdle(this, new EventArgs());
            }
        }

        // A test
        public event EventHandler? CircuitIdle;
        public bool IsIdle => !timer.Enabled;

        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next)
        {
            return context =>
            {


                timer.Stop();
                timer.Start();
                return next(context);
            };
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }

    //public class IdleCircuitOptions
    //{
    //    public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromSeconds(5);
    //}

    public static class IdleCircuitHandlerServiceCollectionExtensions
    {
        //public static IServiceCollection AddIdleCircuitHandler(
        //    this IServiceCollection services,
        //    Action<IdleCircuitOptions> configureOptions)
        //{
        //    services.Configure(configureOptions);
        //    services.AddIdleCircuitHandler();
        //    return services;
        //}

        public static IServiceCollection AddIdleCircuitHandler(
            this IServiceCollection services)
        {
            services.AddScoped<CircuitHandler, IdleCircuitHandler>();
            return services;
        }
    }
}
