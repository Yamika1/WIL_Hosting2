using System.ComponentModel.DataAnnotations;

namespace ReCenterHub.Models
{
    public class WorkshopBooking
    {
        [Key]
        public int WorkshopBookingID { get; set; }
        public string? UserId { get; set; }

        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }

        [Display(Name = "Target Audience")]
        public string TargetAudience { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime Date_and_Time { get; set; }

        [Display(Name = "Optional Notes")]
        public string? OptionalNotes { get; set; }
      
        [Display(Name = "Topic")]
        public string? Topic { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }
    }
}
