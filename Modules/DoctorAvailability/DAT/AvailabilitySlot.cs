using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAvailability.DAT
{
    public class AvailabilitySlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; }
        public bool IsReserved { get; set; }
        public decimal Cost { get; set; }
    }
}
