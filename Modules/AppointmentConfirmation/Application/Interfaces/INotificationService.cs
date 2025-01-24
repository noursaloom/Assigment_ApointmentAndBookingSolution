
namespace AppointmentConfirmation.Application.Interfaces
    {
    public interface INotificationService
        {
        void SendConfirmation(string message);
        }
    }
