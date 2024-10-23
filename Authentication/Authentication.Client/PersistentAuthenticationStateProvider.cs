using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Authentication.Client
{
    // This is a client-side AuthenticationStateProvider that determines the user's authentication state by
    // looking for data persisted in the page when it was rendered on the server. This authentication state will
    // be fixed for the lifetime of the WebAssembly application. So, if the user needs to log in or out, a full
    // page reload is required.
    //
    // This only provides a user name and email for display purposes. It does not actually include any tokens
    // that authenticate to the server when making subsequent requests. That works separately using a
    // cookie that will be included on HttpClient requests to the server.
    internal class PersistentAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

        // This constructor is called by the server-side code to create the provider with the user's
        // authentication state. The state is passed in as a JSON-serialized string.
        // If the state is missing or invalid, the provider will remain unauthenticated.
        // For more information, see https://docs.microsoft.com/aspnet/core/security/blazor/?view=aspnetcore-5.0&tabs=visual-studio#server-side-blazor-with-cookie-authentication

        //CZ:
        // Tento konstruktor je volán kódem na stranì serveru k vytvoøení poskytovatele s uživatelem
        // stav autentizace. Stav je pøedán jako øetìzec serializovaný JSON.
        // Pokud stav chybí nebo je neplatný, poskytovatel zùstane neovìøený.



        public PersistentAuthenticationStateProvider(PersistentComponentState state)
        {
            if(!state.TryTakeFromJson<UserInfo>(nameof(UserInfo),out var userInfo) || userInfo is null)
            {
                return;
            }

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email) ];

            authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;
    }
}
