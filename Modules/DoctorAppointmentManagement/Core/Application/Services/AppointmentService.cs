using Shared.Entities;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Core.Application.Services
    {
    public class AppointmentService
        {
        private readonly IRepository<Appointments> _appointmentRepository;

        public AppointmentService(IRepository<Appointments> appointmentRepository)
            {
            _appointmentRepository = appointmentRepository;
            }

        public async Task<IEnumerable<Appointments>> GetAppointmentsAsync()
            {
            return await _appointmentRepository.GetAllAsync();
            }

        public async Task BookAppointmentAsync(Appointments appointment)
            {
            await _appointmentRepository.AddAsync(appointment);
            }
        }
    }
