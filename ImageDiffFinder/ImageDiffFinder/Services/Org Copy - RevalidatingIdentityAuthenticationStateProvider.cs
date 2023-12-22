namespace ImageDiffFinder.Services
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Server;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Tutorial links:
    /// https://www.youtube.com/watch?v=42O7rECc87o
    /// https://github.com/dotnet/aspnetcore/blob/release/7.0/src/ProjectTemplates/Web.ProjectTemplates/content/BlazorServerWeb-CSharp/Areas/Identity/RevalidatingIdentityAuthenticationStateProvider.cs
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-8.0&tabs=visual-studio#additional-security-abstractions
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class OrgCopyRevalidatingIdentityAuthenticationStateProvider<TUser>
        : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public OrgCopyRevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Get the user manager from a new scope to ensure it fetches fresh data
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                return await ValidateSecurityStampAsync(userManager, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user == null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
    }
}
