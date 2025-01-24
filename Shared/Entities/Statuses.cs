using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Statuses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string? StatusDesc { get; set; }

    }
}
