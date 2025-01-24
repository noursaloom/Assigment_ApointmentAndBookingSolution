using System;
using Moq;
using Xunit;
using AppointmentConfirmation.Application.Interfaces;
using Shared.Entities;
using AppointmentConfirmation.Application.Services;

namespace AppointmentConfirmation.Application.Tests.Services
    {
    public class AppointmentConfirmationServiceTests
        {
        private readonly Mock<ISlotRepository> _slotRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly AppointmentConfirmationService _service;

        public AppointmentConfirmationServiceTests()
            {
            _slotRepositoryMock = new Mock<ISlotRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _notificationServiceMock = new Mock<INotificationService>();

            _service = new AppointmentConfirmationService(
                _slotRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _notificationServiceMock.Object
            );
            }

        [Fact]
        public void ConfirmAppointment_ShouldThrowException_WhenSlotDoesNotExist()
            {
            // Arrange
            Guid slotId = Guid.NewGuid();
            _slotRepositoryMock.Setup(repo => repo.GetSlotById(slotId)).Returns((AvailabilitySlot)null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _service.ConfirmAppointment(slotId, Guid.NewGuid(), "Patient Name"));
            }

        [Fact]
        public void ConfirmAppointment_ShouldThrowException_WhenSlotIsReserved()
            {
            // Arrange
            Guid slotId = Guid.NewGuid();
            var reservedSlot = new AvailabilitySlot
                {
                Id = slotId,
                IsReserved = true
                };
            _slotRepositoryMock.Setup(repo => repo.GetSlotById(slotId)).Returns(reservedSlot);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _service.ConfirmAppointment(slotId, Guid.NewGuid(), "Patient Name"));
            }

        [Fact]
        public void ConfirmAppointment_ShouldReserveSlotAndSaveAppointment_WhenSlotIsAvailable()
            {
            // Arrange
            Guid slotId = Guid.NewGuid();
            var availableSlot = new AvailabilitySlot
                {
                Id = slotId,
                IsReserved = false,
                DoctorName = "Dr. Noor",
                Time = DateTime.UtcNow,
                Cost = 100
                };
            _slotRepositoryMock.Setup(repo => repo.GetSlotById(slotId)).Returns(availableSlot);

            // Act
            _service.ConfirmAppointment(slotId, Guid.NewGuid(), "Patient Name");

            // Assert
            Assert.True(availableSlot.IsReserved);
            _slotRepositoryMock.Verify(repo => repo.UpdateSlot(availableSlot), Times.Once);
            _appointmentRepositoryMock.Verify(repo => repo.Save(It.IsAny<Appointments>()), Times.Once);
            _notificationServiceMock.Verify(service =>
                service.SendConfirmation(It.Is<string>(msg =>
                    msg.Contains("Patient: Patient Name") &&
                    msg.Contains("Doctor: Dr. Smith") &&
                    msg.Contains("Cost: $100"))),
                Times.Once);
            }

        [Fact]
        public void ConfirmAppointment_ShouldSendNotification_WhenAppointmentIsConfirmed()
            {
            // Arrange
            Guid slotId = Guid.NewGuid();
            var availableSlot = new AvailabilitySlot
                {
                Id = slotId,
                IsReserved = false,
                DoctorName = "Dr. Noor",
                Time = DateTime.UtcNow,
                Cost = 200
                };
            _slotRepositoryMock.Setup(repo => repo.GetSlotById(slotId)).Returns(availableSlot);

            // Act
            _service.ConfirmAppointment(slotId, Guid.NewGuid(), "John Doe");

            // Assert
            _notificationServiceMock.Verify(service =>
                service.SendConfirmation(It.Is<string>(msg =>
                    msg.Contains("Appointment Confirmation:") &&
                    msg.Contains("Patient: John Doe") &&
                    msg.Contains("Doctor: Dr. Smith") &&
                    msg.Contains($"Cost: ${availableSlot.Cost}"))),
                Times.Once);
            }
        }
    }
