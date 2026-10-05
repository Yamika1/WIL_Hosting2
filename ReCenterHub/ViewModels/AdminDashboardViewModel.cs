using ReCenterHub.Models;

namespace ReCenterHub.ViewModels
{
    public class AdminDashboardViewModel
    {
       public List<IndividualBooking> IndividualBookings { get; set; } = new List<IndividualBooking>();

       public List<WorkshopBooking> WorkshopBookings { get; set; } = new List<WorkshopBooking>();

        public int NewBookingCount { get; set; }
    }
}
