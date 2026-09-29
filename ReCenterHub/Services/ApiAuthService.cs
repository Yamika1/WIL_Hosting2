namespace ReCenterHub.Services
{
    public class ApiAuthService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(
            string email,
            string password)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "login?useCookies=false",
                new
                {
                    email,
                    password
                });

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<bool> RegisterAsync(
            string email,
            string password)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "register",
                new
                {
                    email,
                    password
                });

            return response.IsSuccessStatusCode;
        }
    }

    public class LoginResponse
    {
        public string? TokenType { get; set; }

        public string? AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public string? RefreshToken { get; set; }
    }
}