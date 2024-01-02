namespace ImageDiffFinder.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ImageDiffFinder.Models.Other;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
    using WebUtils;

    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-8.0&source=recommendations&tabs=visual-studio#implement-a-custom-authenticationstateprovider
    /// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/server/?view=aspnetcore-8.0&source=recommendations&tabs=visual-studio#notification-about-authentication-state-changes
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // To show authorizing message, just for test
                await Task.Delay(2000);

                var appState = await BlazorUtils.GetStateAsync<AppState>(_localStorage);
                if (appState == null || string.IsNullOrEmpty(appState.Token))
                    return await Task.FromResult(new AuthenticationState(_anonymous));

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, appState.PersonUsername),
                    new Claim(ClaimTypes.Role, appState.PersonRole),
                }, "CustomAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch // Used when user modified local storage or when prerender is on
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public async Task UpdateAuthenticationState(AppState appState)
        {
            ClaimsPrincipal claimsPrincipal;

            if (appState != null && !string.IsNullOrEmpty(appState.Token))
            {
                await BlazorUtils.SetStateAsync(_localStorage, appState);
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, appState.PersonUsername),
                    new Claim(ClaimTypes.Role, appState.PersonRole)
                }));
            }
            else
            {
                await BlazorUtils.DeleteAppStateAsync<AppState>(_localStorage);
                claimsPrincipal = _anonymous;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
