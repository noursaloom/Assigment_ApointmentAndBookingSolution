using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Business
{
    public interface IAppointmentBooking
    {
        Task<bool> AddAppointments(Appointments _Appointments);
        Task<List<AvailabilitySlot>> GetAvailableSlotsbydoctorID(Guid _doctorId);
        Task<bool> CheckAppointmentsAvailability(Appointments _Appointments);
    }
}
