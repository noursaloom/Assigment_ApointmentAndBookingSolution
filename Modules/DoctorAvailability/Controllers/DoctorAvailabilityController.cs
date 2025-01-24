using DoctorAvailability.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;

namespace DoctorAvailability.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "DoctorAvailability")]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly DoctorAvailabilityService _doctorAvailabilityService;

        public DoctorAvailabilityController(DoctorAvailabilityService doctorAvailabilityService)
        {
            _doctorAvailabilityService = doctorAvailabilityService;
        }

        [HttpPost("AddSlot")]
        public async Task<IActionResult> AddSlot([FromBody] AvailabilitySlot availabilitySlot)
        {
            if (availabilitySlot == null)
            {
                return BadRequest("Invalid availability slot data.");
            }

            int result = await _doctorAvailabilityService.AddSlots(availabilitySlot);
            if (result == 1)
            {
                return Ok("Slot added successfully.");
            }
            else
            {
                return Conflict("Slot already exists.");
            }
        }

        [HttpGet("UpcomingAppointments")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appointments = await _doctorAvailabilityService.GetUpComingAppointments();
            return Ok(appointments);
        }

        [HttpPut("UpdateAppointmentStatus")]
        public async Task<IActionResult> UpdateAppointmentStatus([FromBody] Appointments appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Invalid appointment data.");
            }

            bool isUpdated = await _doctorAvailabilityService.UpdateAppointmentStatus(appointment);
            if (isUpdated)
            {
                return Ok("Appointment status updated successfully.");
            }
            else
            {
                return NotFound("Appointment not found.");
            }
        }
    }
}
