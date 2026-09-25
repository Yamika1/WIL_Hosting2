using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class UpdateWorkshopBookingDTO
    {
        [Key]
        public int WorkshopBookingID { get; set; }

        [Display(Name = "Institution Name")]
        public string InstitutionName { get; set; }
        public int Age { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime Date_and_Time { get; set; }

        [Display(Name = "Optional Notes")]
        public string? OptionalNotes { get; set; }
    }
}
