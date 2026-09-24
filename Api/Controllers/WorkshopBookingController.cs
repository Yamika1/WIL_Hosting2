using Api.Data;
using Api.Models;
using Api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
                FirstName = WorkshopBookingentity.FirstName,
                Surname = WorkshopBookingentity.Surname,
                Age = WorkshopBookingentity.Age,
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

            workshopBooking.FirstName = WorkshopBookingentity.FirstName;
            workshopBooking.Surname = WorkshopBookingentity.Surname;
            workshopBooking.EmailAddress = WorkshopBookingentity.EmailAddress;
            workshopBooking.Age = WorkshopBookingentity.Age;
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


    }
}