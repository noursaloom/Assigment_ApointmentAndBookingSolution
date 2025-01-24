using AppointmentBooking.Business;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace AppointmentBooking.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "AppointmentBooking")]
    public class AppointmentBookingController : ControllerBase
        {
        private readonly IAppointmentBooking _appointmentBooking;

        public AppointmentBookingController(IAppointmentBooking appointmentBooking)
            {
            _appointmentBooking = appointmentBooking;
            }

        // POST: api/AppointmentBooking/AddAppointment
        [HttpPost("AddAppointment")]
        public async Task<IActionResult> AddAppointment([FromBody] Appointments appointment)
            {
            if (appointment == null)
                return BadRequest("Invalid appointment data.");

            bool result = await _appointmentBooking.AddAppointments(appointment);

            if (result)
                return Ok("Appointment added successfully.");
            else
                return Conflict("The selected time slot is not available.");
            }

        // GET: api/AppointmentBooking/GetAvailableSlots/{doctorId}
        [HttpGet("GetAvailableSlots/{doctorId}")]
        public async Task<IActionResult> GetAvailableSlots(Guid doctorId)
            {
            if (doctorId == Guid.Empty)
                return BadRequest("Invalid doctor ID.");

            var availableSlots = await _appointmentBooking.GetAvailableSlotsbydoctorID(doctorId);

            if (availableSlots == null || availableSlots.Count == 0)
                return NotFound("No available slots found for the specified doctor.");

            return Ok(availableSlots);
            }

        // GET: api/AppointmentBooking/CheckAvailability
        [HttpPost("CheckAvailability")]
        public async Task<IActionResult> CheckAvailability([FromBody] Appointments appointment)
            {
            if (appointment == null)
                return BadRequest("Invalid appointment data.");

            bool isAvailable = await _appointmentBooking.CheckAppointmentsAvailability(appointment);

            return Ok(new { Available = isAvailable });
            }
        }
    }


