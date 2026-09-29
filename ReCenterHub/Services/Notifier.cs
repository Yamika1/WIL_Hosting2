namespace ReCenterHub.Services
{
    public class Notifier
    {
        private List<IObserver> _observers = new();

        public void Subscribe(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(int newBookingCount)
        {
            string message = $"You now have {newBookingCount} new booking/s.";

            foreach (var observer in _observers)
            {
                observer.Update(newBookingCount);
            }
            
        }


    }
}