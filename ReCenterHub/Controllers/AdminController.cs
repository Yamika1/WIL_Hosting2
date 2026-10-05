using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReCenterHub.Models;
using ReCenterHub.Services;
using ReCenterHub.ViewModels;

namespace ReCenterHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IndividualBookingService _individualBookingService;
        private readonly WorkshopBookingService _workshopBookingService;

        public AdminController(
            IndividualBookingService individualBookingService,
            WorkshopBookingService workshopBookingService)
        {
            _individualBookingService = individualBookingService;
            _workshopBookingService = workshopBookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var individualBookings =
                    await _individualBookingService.GetAllIndividualBookingsAsync();

                var workshopBookings =
                    await _workshopBookingService.GetAllWorkshopBookingsAsync();

                var model = new AdminDashboardViewModel
                {
                    IndividualBookings =
                        individualBookings ?? new List<IndividualBooking>(),

                    WorkshopBookings =
                        workshopBookings ?? new List<WorkshopBooking>()
                };

                model.NewBookingCount =
                    model.IndividualBookings.Count(b =>
                        string.Equals(
                            b.Status,
                            BookingStatuses.Pending,
                            StringComparison.OrdinalIgnoreCase))
                    +
                    model.WorkshopBookings.Count(b =>
                        string.Equals(
                            b.Status,
                            BookingStatuses.Pending,
                            StringComparison.OrdinalIgnoreCase));

                if (individualBookings == null ||
                    workshopBookings == null)
                {
                    TempData["Error"] =
                        "One or more booking lists could not be loaded from the API.";
                }

                return View(model);
            }
            catch (ApiUnauthorizedException)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new
                    {
                        returnUrl = Url.Action(
                            nameof(Index),
                            "Admin")
                    });
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "The API could not be reached. Make sure the API is running.";

                return View(new AdminDashboardViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetIndividualStatus(
            int id,
            string status)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (!BookingStatuses.IsValid(status))
            {
                TempData["Error"] =
                    "The selected individual booking status is not valid.";

                return RedirectToAction(nameof(Index));
            }

            var booking =
                await _individualBookingService
                    .GetIndividualBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["Error"] =
                    "The individual booking could not be found.";

                return RedirectToAction(nameof(Index));
            }

            booking.Status = status;

            var updated =
                await _individualBookingService.UpdateAsync(booking);

            TempData[updated != null ? "Notification" : "Error"] =
                updated != null
                    ? "The individual booking status was updated successfully."
                    : "The individual booking status could not be updated.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetWorkshopStatus(
            int id,
            string status)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            if (!BookingStatuses.IsValid(status))
            {
                TempData["Error"] =
                    "The selected workshop booking status is not valid.";

                return RedirectToAction(nameof(Index));
            }

            var booking =
                await _workshopBookingService
                    .GetWorkshopBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["Error"] =
                    "The workshop booking could not be found.";

                return RedirectToAction(nameof(Index));
            }

            booking.Status = status;

            var updated =
                await _workshopBookingService.UpdateAsync(booking);

            TempData[updated != null ? "Notification" : "Error"] =
                updated != null
                    ? "The workshop booking status was updated successfully."
                    : "The workshop booking status could not be updated.";

            return RedirectToAction(nameof(Index));
        }
    }
}