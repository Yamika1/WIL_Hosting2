using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using ReCenterHub.Models;
using ReCenterHub.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ReCenterHub_Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }



        [Fact]
        public async Task Test1_GetAllIndividualBookings()
        {
            //Arrange
            var client = _factory.CreateClient();

            // Act
            var allIndividualBookings = await client.GetAsync("/api/IndividualBooking/client-bookings");
           
            // Assert
            Assert.False(allIndividualBookings.IsSuccessStatusCode);


        }



        [Fact]
        public async Task Test2_DeleteClient()
        {
            // Arrange
            var client = _factory.CreateClient();
            IndividualBooking individualBookingToDelete = new IndividualBooking
            {
                UserId = "test-user-id",
                FirstName = "John",
                Surname = "Doe",
                Category = "Addiction",
                EmailAddress = "johndoe@email.com",
                PhoneNumber = "1234567890",
                Date_and_Time = DateTime.Now,
                OptionalNotes = "I write on behalf of patient, he needs help but he won't admit it, it's been hard dealing with him." +
                                " Outside help might be the only option now",
                Status = "Completed"

            };
            var createdIndividualBookingToDelete = await client.PostAsJsonAsync("/api/IndividualBooking/", individualBookingToDelete);
            

            
            // Act  
            var deletedIndividualBooking = await client.DeleteAsync($"/api/IndividualBooking/{createdIndividualBookingToDelete}");

            // Assert   
            Assert.False(deletedIndividualBooking.IsSuccessStatusCode);
        }


        [Fact]
        public async Task Test3_UpdateIndividualBooking()
        {
            // Arrange
            var client = _factory.CreateClient();
            IndividualBooking individualBookingToUpdate = new IndividualBooking
            {
                UserId = "test-user-id2",
                FirstName = "Jane",
                Surname = "Smith",
                Category = "Anxiety",
                EmailAddress = "janesmith@email.com",
                PhoneNumber = "0987654321",
                Date_and_Time = DateTime.Now.AddDays(5),
                OptionalNotes = "Hello, I'm scheduling this appointment because I fear my anxiety is affecting my relationship and work," +
                                " I don't think I can manage on my own anymore",
                Status = "Scheduled"
            };
            var createdIndividualBookingToUpdate = await client.PostAsJsonAsync("/api/IndividualBooking/", individualBookingToUpdate);
           

            // Act
          
            var updateResult = await client.PutAsJsonAsync($"/api/IndividualBooking/{createdIndividualBookingToUpdate}", individualBookingToUpdate);

            // Assert
            Assert.False(updateResult.IsSuccessStatusCode);
        }


        [Fact]
        public async Task Test4_CreateWorkshopBooking()
        {
            // Arrange
            var client = _factory.CreateClient();
            WorkshopBooking newWorkshopBooking = new WorkshopBooking
            {
                UserId = "test-user-id3",
                InstitutionName = "Tillsberry HighSchool",
                TargetAudience = "Highschool Students",
                EmailAddress = "tillsberry@email.com",
                PhoneNumber = "1234567890",
                Date_and_Time = DateTime.Now.AddDays(12),
                OptionalNotes = "We are interested in hosting a workshop on mental health awareness for our students.",
                Topic = "Mental Health Awareness",
                Status = "Scheduled"

            };

            // Act
            var createdWorkshopBooking = await client.PostAsJsonAsync("/api/WorkshopBooking/", newWorkshopBooking);
            var fetchedWorkshopBooking = await client.GetAsync($"/api/WorkshopBooking/{createdWorkshopBooking}");

          


            // Assert
            Assert.NotNull(fetchedWorkshopBooking);
            


        }


        [Fact]
        public async Task Test5_CreateIndividualBooking()
        {
            // Arrange
            var client = _factory.CreateClient();
            IndividualBooking newIndividualBooking = new IndividualBooking
            {
                UserId = "test-user-id4",
                FirstName = "June",
                Surname = "Silas",
                Category = "Self Development",
                EmailAddress = "junesilas@email.com",
                PhoneNumber = "1298740137",
                Date_and_Time = DateTime.Now.AddDays(12),
                OptionalNotes = "Hello, I am interested in booking a session in order to get help with " +
                "improving aspects about myself that I'm having trouble accomplishing myself, such as productivity.",
                Status = "Scheduled"

            };

            // Act
            var createdIndividualBooking = await client.PostAsJsonAsync("/api/IndividualBooking/", newIndividualBooking);
            var fetchedIndividualBooking = await client.GetAsync($"/api/IndividualBooking/{createdIndividualBooking}");
            
         

            // Assert
            Assert.NotNull(fetchedIndividualBooking);
           


        }





    }
}
