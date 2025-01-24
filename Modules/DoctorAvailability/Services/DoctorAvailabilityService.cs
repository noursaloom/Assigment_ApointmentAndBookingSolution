using Shared.Entities;
using Shared.Repositories;
using Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.SharedEnums;

namespace DoctorAvailability.Services
    {
    public class DoctorAvailabilityService : IDoctorAvailabilityService
    {
       

        private readonly IRepository<Appointments> _AppointmentRepository;
        private readonly IRepository<AvailabilitySlot> _AvailabilitySlotRepository;

        private readonly IUnitOfWork _IUnitOfWork;

        public DoctorAvailabilityService(IUnitOfWork oIUnitOfWork,
            IRepository<AvailabilitySlot> availabilitySlotRepository,
            IRepository<Appointments> appointmentRepository)
        {
            _IUnitOfWork = oIUnitOfWork;
            _AvailabilitySlotRepository = availabilitySlotRepository;
            _AppointmentRepository = appointmentRepository;
        }

        public async Task<int> AddSlots(AvailabilitySlot oAvailabilitySlot)
        {
            if (await CheckSlotsAvailability(oAvailabilitySlot))
            {
                await _AvailabilitySlotRepository.AddAsync(oAvailabilitySlot);
                await _IUnitOfWork.SaveChangesAsync();
                return 1;
            }
            else
                return -1;
        }

        public async Task<bool> CheckSlotsAvailability(AvailabilitySlot oAvailabilitySlot)
        {
            IEnumerable<AvailabilitySlot> lstAvailabilitySlot = await _AvailabilitySlotRepository.GetAllAsync();
            AvailabilitySlot objAvailabilitySlot = lstAvailabilitySlot.FirstOrDefault(e => e.Time == oAvailabilitySlot.Time && e.DoctorId == oAvailabilitySlot.DoctorId);
            return objAvailabilitySlot == null;
        }

        public async Task<IEnumerable<Appointments>> GetUpComingAppointments()
        {
            IEnumerable<Appointments> IenAppointments = await _AppointmentRepository.GetAllAsync();
            return IenAppointments.Where(e => e.StatusId == (int)AppointmentEnum.UpComing);
        }

        public async Task<bool> UpdateAppointmentStatus(Appointments oAppointment)
        {
            try
            {
                Appointments objAppointment = await _AppointmentRepository.GetByIdAsync(oAppointment.Id);
                if (objAppointment == null)
                {
                    return false;
                }

                objAppointment.StatusId = oAppointment.StatusId;
                _AppointmentRepository.Update(objAppointment);

                await _IUnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
