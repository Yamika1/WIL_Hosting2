using ReCenterHub.Models;

namespace ReCenterHub.Services
{
    public class WorkshopBookingService
    {
        private readonly HttpClient _httpClient;

        public WorkshopBookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<WorkshopBooking?> CreateAsync(WorkshopBooking request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/WorkshopBooking/", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<WorkshopBooking>();

        }

        public async Task<List<WorkshopBooking>?> GetAllWorkshopBookingsAsync()
        {
            var response = await _httpClient.GetAsync("api/WorkshopBooking/");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<List<WorkshopBooking>>();

        }

        public async Task<WorkshopBooking?> UpdateAsync(WorkshopBooking request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/WorkshopBooking/{request.WorkshopBookingID}", request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<WorkshopBooking>();

        }

        public async Task<bool> Delete(WorkshopBooking request)
        {
            var response = await _httpClient.DeleteAsync($"api/WorkshopBooking/{request.WorkshopBookingID}");
            return response.IsSuccessStatusCode;
        }

        public async Task<WorkshopBooking?> GetWorkshopBookingByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/WorkshopBooking/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WorkshopBooking>();
            }
            return null;

        }

        public async Task<List<IndividualBooking>?> SearchByInstitutionName(string institutionName)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/SearchByInstitutionName?institutionName={institutionName}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }

        public async Task<List<IndividualBooking>?> FilterByTopic(string topic)
        {
            var response = await _httpClient.GetAsync($"api/IndividualBooking/FilterByTopic?topic={topic}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<IndividualBooking>>();
            }
            return null;
        }




    }
}
