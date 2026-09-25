using ReCenterHub.Models;
using System.Diagnostics.Contracts;

namespace ReCenterHub.Services
{
    public class IndividualBookingService
    {
        private readonly HttpClient _httpClient;

        public IndividualBookingService(HttpClient httpClient)
        {
             _httpClient = httpClient;

        }

        public async Task<IndividualBooking?> CreateAsync(IndividualBooking request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/IndividualBooking/", request);
             if (!response.IsSuccessStatusCode) 
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<IndividualBooking>();

        }

        public async Task<List<IndividualBooking>?> GetAllIndividualBookingsAsync()
        {
            var response = await _httpClient.GetAsync("api/IndividualBooking/");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();

        }

        public async Task<IndividualBooking?> UpdateAsync(IndividualBooking request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/IndividualBooking/{request.IndividualBookingID}", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<IndividualBooking>();

        }

        public async Task<bool> Delete(IndividualBooking request)
        {
            var response = await _httpClient.DeleteAsync($"api/IndividualBooking/{request.IndividualBookingID}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IndividualBooking?> GetIndividualBookingByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IndividualBooking>();
            }
            return null;

        }

        public async Task<List<IndividualBooking>?> SearchByFirstNameAndSurname(string firstName, string surname)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/SearchByFirstNameAndSurname?firstName={firstName}&surname={surname}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }

        public async Task<List<IndividualBooking>?> FilterByCategory(string category)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/FilterByCategory?category={category}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }






    }
}
