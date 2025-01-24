using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Core.Domain.Interfaces
    {
    public interface IAppointmentManagementService
        {
        Task CompletedAppointmentAsync(int appointmentId);
        Task CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<Appointments>> GetAllUpComingAppointmentsAsync();
        }
    }
