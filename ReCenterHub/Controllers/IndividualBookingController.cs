using Microsoft.AspNetCore.Mvc;
using ReCenterHub.Models;
using ReCenterHub.Services;
using System.Net.NetworkInformation;

namespace ReCenterHub.Controllers
{
    public class IndividualBookingController : Controller
    {
        private readonly IndividualBookingService _ibs;  

        public IndividualBookingController(IndividualBookingService ibs)
        {
            _ibs = ibs;
        }

        public IActionResult Index(string? category, string? firstName, string? surname)
        {
            var getAllBookings = _ibs.GetAllIndividualBookingsAsync();

            if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(surname))
            {
                var bookings = _ibs.SearchByFirstNameAndSurname(firstName, surname);
                return View(bookings);
            }

            if (!string.IsNullOrEmpty(category))
            {
                var bookings =  _ibs.FilterByCategory(category);
                return View(bookings);
            }

            return View(getAllBookings);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, Surname,Category,PhoneNumber,EmailAddress,Date_and_Time,OptionalNotes")] IndividualBooking ib)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _ibs.CreateAsync(ib);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return View(ib);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var booking = _ibs.GetIndividualBookingByIdAsync(id.Value);
            return View(booking);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [Bind("FirstName, Surname,Category,PhoneNumber,EmailAddress,Date_and_Time,OptionalNotes")] IndividualBooking ib)
        {
            try
            {
                await _ibs.UpdateAsync(ib);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View(ib);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var booking = await _ibs.GetIndividualBookingByIdAsync(id.Value);

            if (booking == null)
            {
                return NotFound();
            }
            await _ibs.Delete(booking);
            return RedirectToAction(nameof(Index));
        }
    }
}





