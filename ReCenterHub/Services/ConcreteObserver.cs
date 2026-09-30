namespace ReCenterHub.Services
{
    public class ConcreteObserver
    {
        public class Notification : IObserver
        {
            private int _newBookingCount;

            public Notification(int newBookingCount)
            {
                _newBookingCount = newBookingCount;
            }

            public string Update(int newBookingCount)
            {
                string message = $"You now have {_newBookingCount} new booking/s.";
                return message;
            }
        }
    }
}
    
