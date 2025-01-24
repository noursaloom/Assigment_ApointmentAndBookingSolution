using AppointmentConfirmation.Application.Interfaces;
using Shared.Entities;


namespace AppointmentConfirmation.Infrastructure
    {
    public class SlotRepository : ISlotRepository
        {
        private readonly List<AvailabilitySlot> _slots = new();

        public AvailabilitySlot GetSlotById(Guid slotId)
            {
            return _slots.FirstOrDefault(s => s.Id == slotId);
            }

        public void UpdateSlot(AvailabilitySlot slot)
            {
            var index = _slots.FindIndex(s => s.Id == slot.Id);
            if (index != -1)
                _slots[index] = slot;
            }
        }
    }
