using DoctorAppointmentManagement.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace DoctorAppointmentManagement.Adapters.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "DoctorAppointmentManagement")]
    public class DoctorAppointmentManagementController : ControllerBase
        {
        private readonly AppointmentService _appointmentService;
        private readonly AppointmentManagementService _appointmentManagementService;

        public DoctorAppointmentManagementController(AppointmentService appointmentService, AppointmentManagementService appointmentManagementService)
            {
            _appointmentService = appointmentService;
            _appointmentManagementService = appointmentManagementService;
            }

        [HttpGet]
        [Route("GetAllUpComingAppointmentsAsync")]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetAllUpComingAppointmentsAsync()
            {
            var appointments = await _appointmentManagementService.GetAllUpComingAppointmentsAsync();
            if (appointments == null || !appointments.Any())
                {
                return NotFound("No appointments found.");
                }
            return Ok(appointments);
            }

        [HttpGet]
        [Route("GetAppointments")]
        public async Task<ActionResult<IEnumerable<Appointments>>> GetAppointments()
            {
            var appointments = await _appointmentService.GetAppointmentsAsync();
            if (appointments == null || !appointments.Any())
                {
                return NotFound("No appointments found.");
                }
            return Ok(appointments);
            }

        [HttpPost]
        [Route("BookAppointment")]
        public async Task<IActionResult> BookAppointment([FromBody] Appointments appointment)
            {
            if (appointment == null)
                {
                return BadRequest("Appointment data is null.");
                }

            await _appointmentService.BookAppointmentAsync(appointment);
            return CreatedAtAction(nameof(GetAllUpComingAppointmentsAsync), new { id = appointment.Id }, appointment);
            }

        [HttpPut]
        [Route("CancelAppointment/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
            {
            await _appointmentManagementService.CancelAppointmentAsync(appointmentId);
            return NoContent(); // No content to return on successful update
            }

        [HttpPut]
        [Route("CompleteAppointment/{appointmentId}")]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
            {
            await _appointmentManagementService.CompletedAppointmentAsync(appointmentId);
            return NoContent(); // No content to return on successful update
            }
        }

    }
