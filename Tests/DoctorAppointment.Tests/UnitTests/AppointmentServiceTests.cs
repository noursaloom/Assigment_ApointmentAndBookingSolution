using DoctorAppointmentManagement.Core.Application.Services;
using Moq;
using Shared.Entities;
using Shared.Repositories;

namespace DoctorAppointmentManagement.Core.Tests.Application.Services
    {
    public class AppointmentServiceTests
        {
        private readonly Mock<IRepository<Appointments>> _appointmentRepositoryMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
            {
            _appointmentRepositoryMock = new Mock<IRepository<Appointments>>();
            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object);
            }

        [Fact]
        public async Task GetAppointmentsAsync_ShouldReturnAllAppointments()
            {
            // Arrange
            var appointments = new List<Appointments>
            {
                new Appointments { Id = Guid.NewGuid(), PatientName = "Ahmad Salim", ReservedAt = DateTime.UtcNow },
                new Appointments { Id = Guid.NewGuid(), PatientName = "Sami Ahmad", ReservedAt = DateTime.UtcNow.AddDays(1) }
            };
            _appointmentRepositoryMock.Setup(repo => repo.GetAllAsync())
                                      .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentService.GetAppointmentsAsync();

            // Assert
            Assert.Equal(appointments.Count, result.Count());
            Assert.Equal(appointments, result);
            _appointmentRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            }

        [Fact]
        public async Task BookAppointmentAsync_ShouldAddAppointment()
            {
            // Arrange
            var appointment = new Appointments
                {
                Id = Guid.NewGuid(),
                PatientName = "Ibtisam",
                ReservedAt = DateTime.UtcNow
                };
            _appointmentRepositoryMock.Setup(repo => repo.AddAsync(appointment))
                                      .Returns(Task.CompletedTask);

            // Act
            await _appointmentService.BookAppointmentAsync(appointment);

            // Assert
            _appointmentRepositoryMock.Verify(repo => repo.AddAsync(appointment), Times.Once);
            }
        }
    }
