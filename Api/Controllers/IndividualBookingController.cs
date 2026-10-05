using Api.Data;
using Api.Models;
using Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualBookingController : ControllerBase
    {
        private readonly AuthDbContext _authDbContext;

        public IndividualBookingController(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }



        [HttpGet("client-bookings")]
        [Authorize(Roles = "Client")]
        public IActionResult GetClientBookings()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var bookings = _authDbContext.IndividualBooking
                .Where(b => b.UserId == userId)
                .ToList();
            return Ok(bookings);

        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult AddIndividualBookings(AddIndividualBookingDTO individualBookingentity)
        {
            
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var individualBooking = new IndividualBooking()
                {
                    UserId = userId,
                    FirstName = individualBookingentity.FirstName,
                    Surname = individualBookingentity.Surname,
                    Category = individualBookingentity.Category,
                    PhoneNumber = individualBookingentity.PhoneNumber,
                    EmailAddress = individualBookingentity.EmailAddress,
                    Date_and_Time = individualBookingentity.Date_and_Time,
                    OptionalNotes = individualBookingentity.OptionalNotes
                };

                _authDbContext.IndividualBooking.Add(individualBooking);
                _authDbContext.SaveChanges();
                return Ok(individualBookingentity);
            }

            [HttpPost(ActionName="Update")]
            [Route("{id:int}")]
            [Authorize(Roles = "Client")]
            public IActionResult UpdateBooking(int id, UpdateIndividualBookingDTO individualBookingentity)
            {
             
                var individualBooking = _authDbContext.IndividualBooking.Find(id);

                if (individualBooking is null)
                    return NotFound();

                individualBooking.FirstName = individualBookingentity.FirstName;
                individualBooking.Surname = individualBookingentity.Surname;
                individualBooking.EmailAddress = individualBookingentity.EmailAddress;
                individualBooking.Category = individualBookingentity.Category;
                individualBooking.PhoneNumber = individualBookingentity.PhoneNumber;
                individualBooking.Date_and_Time = individualBookingentity.Date_and_Time;
                individualBooking.OptionalNotes = individualBookingentity.OptionalNotes;

                _authDbContext.SaveChanges();
                return Ok(individualBooking);
            }

            [HttpPost(ActionName="Delete")]
            [Route("{id:int}")]
            [Authorize(Roles = "Client")]
            public IActionResult DeleteBooking(int id)
            {
                var indivdualBooking = _authDbContext.IndividualBooking.Find(id);

                if (indivdualBooking is null)
                    return NotFound();

                _authDbContext.IndividualBooking.Remove(indivdualBooking);
                _authDbContext.SaveChanges();
                return Ok();
            }

        [HttpGet("search-by-name")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<IndividualBooking> SearchByFirstNameAndSurname(string firstName, string surname)
        {
            var statusQuery = from booking in _authDbContext.IndividualBooking select booking;
            var searchResults = statusQuery.Where(c => c.FirstName == firstName && c.Surname == surname);
            return searchResults.ToList();
        }
        [HttpGet("filter-by-category")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<IndividualBooking> FilterByCategory(string category)
        {
            var statusQuery = from booking in _authDbContext.IndividualBooking select booking;
            var searchResults = statusQuery.Where(c => c.Category == category);
            return searchResults.ToList();
        }

    }
    }
