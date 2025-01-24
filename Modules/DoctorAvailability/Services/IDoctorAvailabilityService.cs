using Shared.Entities;
namespace DoctorAvailability.Services
    {
    public interface IDoctorAvailabilityService
        {
        Task<bool> UpdateAppointmentStatus(Appointments oAppointment);
        Task<IEnumerable<Appointments>> GetUpComingAppointments();
        Task<bool> CheckSlotsAvailability(AvailabilitySlot oAvailabilitySlot);
        Task<int> AddSlots(AvailabilitySlot oAvailabilitySlot);
        }
    }
