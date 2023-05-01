using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorTest.Client.Utils
{
    //butun sayfalarda authontatication mekanizmasi kurmak icin. bri isi yapip yapmadigi surekli control edilecek.
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _client;
        private readonly AuthenticationState anonymous;
        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient Client)
        {
            _localStorageService = localStorageService;
            anonymous = new AuthenticationState(new ClaimsPrincipal(
                new ClaimsIdentity()
                )); ;
            _client = Client;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string Token = await _localStorageService.GetItemAsStringAsync("token");
            if (string.IsNullOrEmpty(Token)) return anonymous;
            
            Console.WriteLine($"Token {Token}");
            string UserEmail = await _localStorageService.GetItemAsStringAsync("UserMail");
            Console.WriteLine(UserEmail);
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, UserEmail) }, "jwtAuthType"));

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            return new AuthenticationState(cp);
        }

        //bu metodu lloginde kullanacagiz
        public void NotifyUserLogin(string Email)
        {
            var AuthClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, Email) }, "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(AuthClaimPrincipal));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout(string Email)
        {
            var anonymousTask = Task.FromResult(anonymous);

            NotifyAuthenticationStateChanged(anonymousTask);
        }
    }
}
