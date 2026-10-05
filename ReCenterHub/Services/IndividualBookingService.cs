using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;
using ReCenterHub.Models;
using System.Net;
using System.Net.Http.Headers;
using static ReCenterHub.Services.ConcreteObserver;


namespace ReCenterHub.Services
{
    [Authorize(Roles = "Client,Admin")]
    public class IndividualBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public IndividualBookingService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
             _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            
        }
        private void AddToken()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("AccessToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        private static void ThrowIfUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new ApiUnauthorizedException();
            }
        }

        public async Task<IndividualBooking?> CreateAsync(IndividualBooking request)
        {
            AddToken(); 
            var response = await _httpClient.PostAsJsonAsync("api/IndividualBooking/", request);
             if (!response.IsSuccessStatusCode) 
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<IndividualBooking>();
        }

        public async Task<List<IndividualBooking>?> GetClientBookingsAsync()
        {
            AddToken();

            var response = await _httpClient.GetAsync("api/IndividualBooking/client-bookings");
            ThrowIfUnauthorized(response);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
        }

        public async Task<List<IndividualBooking>?> GetAllIndividualBookingsAsync()
        {
            AddToken();

            var response = await _httpClient.GetAsync("api/IndividualBooking/{id}");
            ThrowIfUnauthorized(response);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();

        }

        public async Task<IndividualBooking?> UpdateAsync(IndividualBooking request)
        {
            AddToken();

            var response = await _httpClient.PutAsJsonAsync(
                $"api/IndividualBooking/{request.IndividualBookingID}", request);
            ThrowIfUnauthorized(response);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<IndividualBooking>();
        }

        public async Task<bool> Delete(IndividualBooking request)
        {
            AddToken();
            var response = await _httpClient.DeleteAsync($"api/IndividualBooking/{request.IndividualBookingID}");
            ThrowIfUnauthorized(response);
            return response.IsSuccessStatusCode;
        }

        public async Task<IndividualBooking?> GetIndividualBookingByIdAsync(int id)
        {

            AddToken();

            var response = await _httpClient.GetAsync($"api/IndividualBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IndividualBooking>();
            }
            return null;

        }

        public async Task<List<IndividualBooking>?> SearchByFirstNameAndSurname(string firstName, string surname)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/search-by-name?firstName={firstName}&surname={surname}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }

        public async Task<List<IndividualBooking>?> FilterByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/filter-by-category?category={category}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }

        public List<IndividualBooking> UpcomingSessions(IEnumerable<IndividualBooking> bookings)
        {
            return bookings.Where(b =>
                b.Date_and_Time > DateTime.Now &&
                (b.Status == "Scheduled" || b.Status == "Rescheduled")
            ).ToList();
        }
    }
}