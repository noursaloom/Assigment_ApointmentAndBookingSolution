using Shared.Entities;
using Shared.Repositories;
using Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.SharedEnums;
using static System.Reflection.Metadata.BlobBuilder;

namespace AppointmentBooking.Business
{
    public class AppointmentBookingService : IAppointmentBooking
    {
        private readonly IRepository<Appointments> _AppointmentRepository;
        private readonly IRepository<AvailabilitySlot> _AvailabilitySlotRepository;
        private readonly IUnitOfWork _IUnitOfWork;

        public AppointmentBookingService(IUnitOfWork oIUnitOfWork,
            IRepository<AvailabilitySlot> availabilitySlotRepository,
            IRepository<Appointments> appointmentRepository)
        {
            _IUnitOfWork = oIUnitOfWork;
            _AvailabilitySlotRepository = availabilitySlotRepository;
            _AppointmentRepository = appointmentRepository;
        }
        public async Task<bool> AddAppointments(Appointments _Appointments)
        {
            if (await CheckAppointmentsAvailability(_Appointments))
            {
                await _AppointmentRepository.AddAsync(_Appointments);
                await _IUnitOfWork.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        public async Task<List<AvailabilitySlot>> GetAvailableSlotsbydoctorID(Guid _doctorId)
        {
            IEnumerable<AvailabilitySlot> lstAppointments = await _AvailabilitySlotRepository.GetAllAsync();
            lstAppointments = lstAppointments.Where(e => e.DoctorId == _doctorId);
            return lstAppointments.ToList();
        }
        public async Task<bool> CheckAppointmentsAvailability(Appointments _Appointments)
        {
            IEnumerable<Appointments> lstAppointments = await _AppointmentRepository.GetAllAsync();
            Appointments objAppointments = lstAppointments.FirstOrDefault(e => e.ReservedAt == _Appointments.ReservedAt && e.StatusId != (int)AppointmentEnum.Completed);
            return objAppointments == null;
        }

     
    }
}
