using ImageDiffFinder.Models.Other;
using System.Globalization;

namespace ImageDiffFinder.Middlewares
{
    public class CustomHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppState appState)
        {
            //context.Request.Headers.Add("MY-TEST-HEADER", "MYTESTHEADER");
            context.Request.Headers.Add("MY-TEST-HEADER", "MYTESTHEADER");
            Console.WriteLine($"My custom header value is: MYTESTHEADER");

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
