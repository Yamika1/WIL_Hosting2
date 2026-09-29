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

            public string Update(string message)
            {
                message = $"You now have {_newBookingCount} new booking/s.";
                return message;
            }
        }
    }
}
    
