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
        private string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private bool CanAccess(IndividualBooking booking) =>
            User.IsInRole("Admin") || booking.UserId == CurrentUserId;



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

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Client,Admin")]
        public IActionResult GetBookingById(int id)
        {
            var booking = _authDbContext.IndividualBooking.Find(id);

            if (booking is null)
                return NotFound();

            if (!CanAccess(booking))
                return StatusCode(StatusCodes.Status403Forbidden);

            return Ok(booking);
        }

        [HttpPost]
        [Authorize(Roles = "Client,Admin")]
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
                    OptionalNotes = individualBookingentity.OptionalNotes,
                    Status = "Scheduled"
                };

                _authDbContext.IndividualBooking.Add(individualBooking);
                _authDbContext.SaveChanges();
                return Ok(individualBookingentity);
            }

            [HttpPost(ActionName="Update")]
            [Route("{id:int}")]
        [Authorize(Roles = "Client,Admin")]
        public IActionResult UpdateBooking(int id, UpdateIndividualBookingDTO individualBookingentity)
            {
             
                var individualBooking = _authDbContext.IndividualBooking.Find(id);

                if (individualBooking is null)
                    return NotFound();

            if (!CanAccess(individualBooking))
                return StatusCode(StatusCodes.Status403Forbidden);


            individualBooking.FirstName = individualBookingentity.FirstName;
                individualBooking.Surname = individualBookingentity.Surname;
                individualBooking.EmailAddress = individualBookingentity.EmailAddress;
                individualBooking.Category = individualBookingentity.Category;
                individualBooking.PhoneNumber = individualBookingentity.PhoneNumber;
                individualBooking.Date_and_Time = individualBookingentity.Date_and_Time;
                individualBooking.OptionalNotes = individualBookingentity.OptionalNotes;


            if (User.IsInRole("Admin") && !string.IsNullOrWhiteSpace(individualBookingentity.Status))
                individualBookingentity.Status = individualBookingentity.Status;

            _authDbContext.SaveChanges();
                return Ok(individualBooking);
            }

            [HttpPost(ActionName="Delete")]
            [Route("{id:int}")]
            [Authorize(Roles = "Client,Admin")]
            public IActionResult DeleteBooking(int id)
            {
                var indivdualBooking = _authDbContext.IndividualBooking.Find(id);

                if (indivdualBooking is null)
                    return NotFound();

            if (!CanAccess(indivdualBooking))
                return StatusCode(StatusCodes.Status403Forbidden);

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
