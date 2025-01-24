using AppointmentConfirmation.Application.Interfaces;
using Shared.Entities;


namespace AppointmentConfirmation.Application.Services
    {
        public class AppointmentConfirmationService
            {
            private readonly ISlotRepository _slotRepository;
            private readonly IAppointmentRepository _appointmentRepository;
            private readonly INotificationService _notificationService;

            public AppointmentConfirmationService(
                ISlotRepository slotRepository,
                IAppointmentRepository appointmentRepository,
                INotificationService notificationService)
                {
                _slotRepository = slotRepository;
                _appointmentRepository = appointmentRepository;
                _notificationService = notificationService;
                }

            public void ConfirmAppointment(Guid slotId, Guid patientId, string patientName)
                {
                var slot = _slotRepository.GetSlotById(slotId);
                if (slot == null || slot.IsReserved)
                    throw new InvalidOperationException("Slot is not available.");

                slot.Reserve();
                _slotRepository.UpdateSlot(slot);

                var appointment = new Appointments(
                    Guid.NewGuid(),
                    slotId,
                    patientId,
                    patientName,
                    DateTime.UtcNow
                );
                _appointmentRepository.Save(appointment);

                var message = $"Appointment Confirmation:\n" +
                              $"Patient: {patientName}\n" +
                              $"Doctor: {slot.DoctorName}\n" +
                              $"Time: {slot.Time}\n" +
                              $"Cost: ${slot.Cost}";
                _notificationService.SendConfirmation(message);
                }
            }
        }
