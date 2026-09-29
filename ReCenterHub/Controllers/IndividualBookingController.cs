using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReCenterHub.Models;
using ReCenterHub.Services;
using System.Net.NetworkInformation;
using static ReCenterHub.Services.ConcreteObserver;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Client")]
    public class IndividualBookingController : Controller
    {
        private readonly IndividualBookingService _ibs;
        private readonly Notifier _notifier;
        public int newBookingCount = 0;

        public IndividualBookingController(IndividualBookingService ibs, Notifier notifier)
        {
            _ibs = ibs;
            _notifier = notifier;
        }

        public async Task<IActionResult> Index(string? category, string? firstName, string? surname)
        {
            var getAllBookings = _ibs.GetAllIndividualBookingsAsync();

            var upcomingSessions = await _ibs.UpcomingSessions();

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
                    var admin = new Notification(newBookingCount);
                    _notifier.Subscribe(admin);

                    await _ibs.CreateAsync(ib);

                    newBookingCount++;
                    _notifier.Notify(newBookingCount);
                    string notificationMessage = " You now have " + newBookingCount + " new booking/s.";
                    TempData["Notification"] = notificationMessage;

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





