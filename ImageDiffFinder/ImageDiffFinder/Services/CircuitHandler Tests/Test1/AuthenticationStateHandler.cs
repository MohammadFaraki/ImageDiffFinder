using Microsoft.AspNetCore.Components.Authorization;

namespace ImageDiffFinder.Services
{
    public class AuthenticationStateHandler : DelegatingHandler
    {
        readonly CircuitServicesAccessor circuitServicesAccessor;

        public AuthenticationStateHandler(
            CircuitServicesAccessor circuitServicesAccessor)
        {
            this.circuitServicesAccessor = circuitServicesAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authStateProvider = circuitServicesAccessor.Services
                .GetRequiredService<AuthenticationStateProvider>();
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is not null && user.Identity.IsAuthenticated)
            {
                request.Headers.Add("X-USER-IDENTITY-NAME", user.Identity.Name);
            }

            // My Test
            request.Headers.Add("MY-TEST-HEADER", "MYTESTHEADER");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
