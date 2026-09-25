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
        private readonly IWebHostEnvironment _environment;

        public WorkshopBookingController(WorkshopBookingService wbs, IWebHostEnvironment environment)
        {
            _wbs = wbs;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var allBookings = _wbs.GetAllWorkshopBookingsAsync();
            return View();
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




