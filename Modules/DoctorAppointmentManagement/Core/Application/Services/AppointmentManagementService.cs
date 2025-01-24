using DoctorAppointmentManagement.Core.Domain.Interfaces;
using Shared.Entities;
using Shared.Repositories;
using Shared.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Enums.SharedEnums;

namespace DoctorAppointmentManagement.Core.Application.Services
    {
    public class AppointmentManagementService : IAppointmentManagementService
        {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Appointments> _Appointments;
        public AppointmentManagementService(IUnitOfWork unitOfWork, IRepository<Appointments> appointments)
            {
            _unitOfWork = unitOfWork;
            _Appointments = appointments;
            }

        public async Task<IEnumerable<Appointments>> GetAllUpComingAppointmentsAsync()
            {
            IEnumerable<Appointments> IenAppointments = await _unitOfWork.Repository<Appointments>().GetAllAsync();
            IenAppointments = IenAppointments.Where(e => e.StatusId == (int)AppointmentEnum.UpComing);
            return IenAppointments;
            }

        public async Task CancelAppointmentAsync(int appointmentId)
            {
            var appointment = await _unitOfWork.Repository<Appointments>().GetByIdAsync(appointmentId);
            if (appointment != null)
                {
                appointment.StatusId = (int)AppointmentEnum.Canceled;
                _Appointments.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
                }
            }
        public async Task CompletedAppointmentAsync(int appointmentId)
            {
            var appointment = await _Appointments.GetByIdAsync(appointmentId);
            if (appointment != null)
                {
                appointment.StatusId = (int)AppointmentEnum.Completed;
                _Appointments.Update(appointment);
                await _unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
