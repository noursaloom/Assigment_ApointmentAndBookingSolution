using AppointmentConfirmation.Application.Interfaces;
using Shared.Entities;


namespace AppointmentConfirmation.Infrastructure
    {
    public class AppointmentRepository : IAppointmentRepository
        {
        private readonly List<Appointments> _appointments = new();

        public void Save(Appointments appointment)
            {
            _appointments.Add(appointment);
            }
        }
    }
