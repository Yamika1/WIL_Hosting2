namespace ReCenterHub.Models
{
    public class BookingStatuses
    {
        public const string Scheduled = "Scheduled";
        public const string Pending = "Pending";
        public const string InProgress = "In Progress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All = { Scheduled, Pending, InProgress, Completed, Cancelled };

        public static bool IsValid(string? status)
        {
            return status is not null && All.Contains(status);
        }

        public static bool isClosed(string? status)
        {
            return status is not null && (status == Completed || status == Cancelled);
        }
    }
}
