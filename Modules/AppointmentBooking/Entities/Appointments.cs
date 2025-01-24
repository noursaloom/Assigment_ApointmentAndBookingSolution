using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Entities
{
    public class Appointments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public DateTime ReservedAt { get; set; }
        public int? StatusId { get; set; }

    }
}
