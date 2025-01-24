using AppointmentConfirmation.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentConfirmation.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "AppointmentConfirmation")]
    public class AppointmentConfirmationController : ControllerBase
        {
        private readonly AppointmentConfirmationService _service;

        public AppointmentConfirmationController(AppointmentConfirmationService service)
            {
            _service = service;
            }

        [HttpPost("confirm")]
        public IActionResult ConfirmAppointment([FromBody] ConfirmAppointmentRequest request)
            {
            _service.ConfirmAppointment(request.SlotId, request.PatientId, request.PatientName);
            return Ok("Appointment confirmed.");
            }
        }
    public class ConfirmAppointmentRequest
        {
        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        }
    }
