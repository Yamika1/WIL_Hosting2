using Microsoft.AspNetCore.Mvc;
using ReCenterHub.Models;
using ReCenterHub.Services;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace ReCenterHub.Controllers
{
    public class WorkshopBookingController : Controller
    {
        private readonly WorkshopBookingService _wbs;
       

        public WorkshopBookingController(WorkshopBookingService wbs)
        {
            _wbs = wbs;  
        }

        public IActionResult Index(string? topic, string? instituitionName)
        {
            var allBookings = _wbs.GetAllWorkshopBookingsAsync();

            if (!string.IsNullOrEmpty(instituitionName))
            {
                var bookings = _wbs.SearchByInstitutionName(instituitionName);
                return View(bookings);
            }

            if (!string.IsNullOrEmpty(topic))
            {
                var bookings = _wbs.FilterByTopic(topic);
                return View(bookings);
            }

            return View(allBookings);
        }
        

           
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutionName, TargetAudience, PhoneNumber,EmailAddress,Date_and_Time,OptionalNotes")] WorkshopBooking wb)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _wbs.CreateAsync(wb);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return View(wb);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = _wbs.GetWorkshopBookingByIdAsync(id.Value);
            return View(booking);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,[Bind("InstitutionName, TargetAudience, PhoneNumber,EmailAddress,Date_and_Time,OptionalNotes")] WorkshopBooking wb)
        {
            try
            {
                await _wbs.UpdateAsync(wb);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(wb);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _wbs.GetWorkshopBookingByIdAsync(id.Value);

            if (booking == null)
            {
                return NotFound();
            }
            await _wbs.Delete(booking);
            return RedirectToAction(nameof(Index));
        }
    }
}




