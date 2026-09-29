using Api.Data;
using Api.Models;
using Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopBookingController : ControllerBase
    {
        private readonly AuthDbContext _authDbContext;

        public WorkshopBookingController(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllWorkshops()
        {
            return Ok(_authDbContext.WorkshopBooking.ToList());
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetWorkshopsById(int id)
        {
            var workshopBooking = _authDbContext.WorkshopBooking.Find(id);

            if (workshopBooking is null)
                return NotFound();

            return Ok(workshopBooking);
        }

        [HttpGet("client-workshops")]
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
        public IActionResult AddWorkshopBookings(AddWorkshopBookingDTO WorkshopBookingentity)
        {
            var WorkshopBooking = new WorkshopBooking()
            {
                InstitutionName = WorkshopBookingentity.InstitutionName,
                TargetAudience = WorkshopBookingentity.TargetAudience,
                Topic = WorkshopBookingentity.Topic,
                Status = WorkshopBookingentity.Status,
                PhoneNumber = WorkshopBookingentity.PhoneNumber,
                EmailAddress = WorkshopBookingentity.EmailAddress,
                Date_and_Time = WorkshopBookingentity.Date_and_Time,
                OptionalNotes = WorkshopBookingentity.OptionalNotes
            };

            _authDbContext.WorkshopBooking.Add(WorkshopBooking);
            _authDbContext.SaveChanges();
            return Ok(WorkshopBookingentity);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateWorkshop(int id, WorkshopBooking WorkshopBookingentity)
        {
            var workshopBooking = _authDbContext.WorkshopBooking.Find(id);

            if (workshopBooking is null)
                return NotFound();

            workshopBooking.InstitutionName = WorkshopBookingentity.InstitutionName;
            workshopBooking.TargetAudience = WorkshopBookingentity.TargetAudience;
            workshopBooking.EmailAddress = WorkshopBookingentity.EmailAddress;
            workshopBooking.Status = WorkshopBookingentity.Status;
            workshopBooking.Topic = WorkshopBookingentity.Topic;
            workshopBooking.PhoneNumber = WorkshopBookingentity.PhoneNumber;
            workshopBooking.Date_and_Time = WorkshopBookingentity.Date_and_Time;
            workshopBooking.OptionalNotes = WorkshopBookingentity.OptionalNotes;

            _authDbContext.SaveChanges();
            return Ok(workshopBooking);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public IActionResult DeleteWorkshopBooking(int id)
        {
            var workshopBooking = _authDbContext.WorkshopBooking.Find(id);

            if (workshopBooking is null)
                return NotFound();

            _authDbContext.WorkshopBooking.Remove(workshopBooking);
            _authDbContext.SaveChanges();
            return Ok();
        }

        [HttpGet("SearchByInstitutionName")]
        public IEnumerable<WorkshopBooking> SearchByInstitutionName(string institutionName)
        {
            var statusQuery = from booking in _authDbContext.WorkshopBooking select booking;
            var searchResults = statusQuery.Include(c => c.WorkshopBookingID).Where(c => c.InstitutionName == institutionName);
            return searchResults.ToList();
        }

        [HttpGet("FilterByTopic")]
        public IEnumerable<WorkshopBooking> FilterByTopic(string topic)
        {
            var statusQuery = from booking in _authDbContext.WorkshopBooking select booking;
            var searchResults = statusQuery.Include(c => c.WorkshopBookingID).Where(c => c.Topic == topic);
            return searchResults.ToList();

        }
        


    }
}