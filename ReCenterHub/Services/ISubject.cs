namespace ReCenterHub.Services
{
   public interface ISubject
        {
            void Subscribe(IObserver observer);
            void Unsubscribe(IObserver observer);
            void Notify();
        }
    }