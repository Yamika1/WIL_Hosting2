using Microsoft.AspNetCore.Http;
using ReCenterHub.Models;
using ReCenterHub.Services;
using System.ComponentModel.DataAnnotations;

namespace ReCenterHub_Tests
{
    public class IntegrationTests
    {
        private IndividualBookingService GetIndividualBookingService()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7182/")
            };

            var httpContextAccessor = new HttpContextAccessor();
            return new IndividualBookingService(httpClient, httpContextAccessor);
        }

        private WorkshopBookingService GetWorkshopBookingService()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7182/")
            };

            var httpContextAccessor = new HttpContextAccessor();
            return new WorkshopBookingService(httpClient, httpContextAccessor);
        }

        [Fact]
        public async Task Test1_GetAllIndividualBookings()
        {
            //Arrange
            IndividualBookingService ibs = GetIndividualBookingService();

            // Act
            var allIndividualBookings = await ibs.GetAllIndividualBookingsAsync();

            // Assert
            Assert.NotNull(allIndividualBookings);
            Assert.IsType<List<IndividualBooking>>(allIndividualBookings);
        }



        [Fact]
        public async Task Test2_DeleteClient()
        {
            // Arrange
            IndividualBookingService ibs = GetIndividualBookingService();
            IndividualBooking individualBookingToDelete = new IndividualBooking
            {
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
            var createdIndividualBookingToDelete = await ibs.CreateAsync(individualBookingToDelete);

            // Act
            bool deleteResult = await ibs.Delete(createdIndividualBookingToDelete);
            var deletedIndividualBooking = await ibs.GetIndividualBookingByIdAsync(createdIndividualBookingToDelete.IndividualBookingID);

            // Assert
            Assert.True(deleteResult);
            Assert.Null(deletedIndividualBooking);
        }


        [Fact]
        public async Task Test3_UpdateIndividualBooking()
        {
            // Arrange
            IndividualBookingService ibs = GetIndividualBookingService();
            IndividualBooking individualBookingToUpdate = new IndividualBooking
            {
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
            var createdIndividualBookingToUpdate = await ibs.CreateAsync(individualBookingToUpdate);

            // Act
            createdIndividualBookingToUpdate.Status = "Rescheduled";
            createdIndividualBookingToUpdate.Date_and_Time = DateTime.Now.AddDays(10);

            var updateResult = await ibs.UpdateAsync(createdIndividualBookingToUpdate);

            // Assert
            Assert.Equal("Rescheduled", updateResult.Status);
            Assert.Equal(DateTime.Now.AddDays(10), updateResult.Date_and_Time);
        }


        [Fact]
        public async Task Test4_CreateWorkshopBookingAndVerifyExistance()
        {
            // Arrange
            WorkshopBookingService workshopBookingService = GetWorkshopBookingService();
            WorkshopBooking newWorkshopBooking = new WorkshopBooking
            {
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
            var createdWorkshopBooking = await workshopBookingService.CreateAsync(newWorkshopBooking);
            var fetchedWorkshopBooking = await workshopBookingService.GetWorkshopBookingByIdAsync(createdWorkshopBooking.WorkshopBookingID);


            // Assert
            Assert.NotNull(fetchedWorkshopBooking);
            Assert.Equal(newWorkshopBooking.InstitutionName, fetchedWorkshopBooking.InstitutionName);
            Assert.Equal(newWorkshopBooking.TargetAudience, fetchedWorkshopBooking.TargetAudience);
            Assert.Equal(newWorkshopBooking.EmailAddress, fetchedWorkshopBooking.EmailAddress);
            Assert.Equal(newWorkshopBooking.PhoneNumber, fetchedWorkshopBooking.PhoneNumber);
            Assert.Equal(newWorkshopBooking.Date_and_Time, fetchedWorkshopBooking.Date_and_Time);
            Assert.Equal(newWorkshopBooking.OptionalNotes, fetchedWorkshopBooking.OptionalNotes);
            Assert.Equal(newWorkshopBooking.Topic, fetchedWorkshopBooking.Topic);
            Assert.Equal(newWorkshopBooking.Status, fetchedWorkshopBooking.Status);
        }





    }
}
