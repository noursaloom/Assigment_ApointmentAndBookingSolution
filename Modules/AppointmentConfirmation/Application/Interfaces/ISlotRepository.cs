using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentConfirmation.Application.Interfaces
    {
    public interface ISlotRepository
    {
        AvailabilitySlot GetSlotById(Guid slotId);
        void UpdateSlot(AvailabilitySlot slot);
    }
    }
