using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
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
        public bool IsReserved { get; private set; }
        public Appointments(Guid guid, Guid slotId, Guid patientId, string patientName, DateTime utcNow)
            {
            Id = guid;
            SlotId = slotId;
            PatientId = patientId;
            PatientName = patientName;
            ReservedAt = utcNow;
            }
        public Appointments()
            {
            }
        public void Reserve()
            {
            IsReserved = true;
            }

        public void CancelReservation()
            {
            IsReserved = false;
            }
        }
}
