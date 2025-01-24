using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentBooking.Business;
using Moq;
using Shared.Entities;
using Shared.Repositories;
using Shared.UnitOfWork;
using Xunit;
using static Shared.Enums.SharedEnums;

public class AppointmentBookingServiceTests
    {
    private readonly Mock<IRepository<Appointments>> _appointmentRepositoryMock;
    private readonly Mock<IRepository<AvailabilitySlot>> _availabilitySlotRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AppointmentBookingService _service;

    public AppointmentBookingServiceTests()
        {
        _appointmentRepositoryMock = new Mock<IRepository<Appointments>>();
        _availabilitySlotRepositoryMock = new Mock<IRepository<AvailabilitySlot>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new AppointmentBookingService(_unitOfWorkMock.Object, _availabilitySlotRepositoryMock.Object, _appointmentRepositoryMock.Object);
        }

    [Fact]
    public async Task AddAppointments_ShouldAddAppointment_WhenSlotIsAvailable()
        {
        // Arrange
        var appointment = new Appointments { ReservedAt = DateTime.UtcNow, StatusId = (int)AppointmentEnum.UpComing };
        _appointmentRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Appointments>());
        _appointmentRepositoryMock
            .Setup(repo => repo.AddAsync(appointment))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(CancellationToken.None))
            .Returns((Task<int>)Task.CompletedTask);

        // Act
        var result = await _service.AddAppointments(appointment);

        // Assert
        Assert.True(result);
        _appointmentRepositoryMock.Verify(repo => repo.AddAsync(appointment), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

    [Fact]
    public async Task AddAppointments_ShouldNotAddAppointment_WhenSlotIsNotAvailable()
        {
        // Arrange
        var appointment = new Appointments { ReservedAt = DateTime.UtcNow, StatusId = (int)AppointmentEnum.UpComing };
        var existingAppointments = new List<Appointments>
        {
            new Appointments { ReservedAt = appointment.ReservedAt, StatusId = (int)AppointmentEnum.UpComing }
        };
        _appointmentRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(existingAppointments);

        // Act
        var result = await _service.AddAppointments(appointment);

        // Assert
        Assert.False(result);
        _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Appointments>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
        }

    [Fact]
    public async Task GetAvailableSlotsbydoctorID_ShouldReturnAvailableSlotsForDoctor()
        {
        // Arrange
        var doctorId = Guid.NewGuid();
        var slots = new List<AvailabilitySlot>
        {
            new AvailabilitySlot { DoctorId = doctorId, Time = DateTime.UtcNow },
            new AvailabilitySlot { DoctorId = Guid.NewGuid(), Time = DateTime.UtcNow }
        };
        _availabilitySlotRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(slots);

        // Act
        var result = await _service.GetAvailableSlotsbydoctorID(doctorId);

        // Assert
        Assert.Single(result);
        Assert.Equal(doctorId, result[0].DoctorId);
        }

    [Fact]
    public async Task CheckAppointmentsAvailability_ShouldReturnTrue_WhenNoConflictingAppointmentExists()
        {
        // Arrange
        var appointment = new Appointments { ReservedAt = DateTime.UtcNow, StatusId = (int)AppointmentEnum.UpComing };
        _appointmentRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Appointments>());

        // Act
        var result = await _service.CheckAppointmentsAvailability(appointment);

        // Assert
        Assert.True(result);
        }

    [Fact]
    public async Task CheckAppointmentsAvailability_ShouldReturnFalse_WhenConflictingAppointmentExists()
        {
        // Arrange
        var appointment = new Appointments { ReservedAt = DateTime.UtcNow, StatusId = (int)AppointmentEnum.UpComing };
        var existingAppointments = new List<Appointments>
        {
            new Appointments { ReservedAt = appointment.ReservedAt, StatusId = (int)AppointmentEnum.UpComing }
        };
        _appointmentRepositoryMock
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(existingAppointments);

        // Act
        var result = await _service.CheckAppointmentsAvailability(appointment);

        // Assert
        Assert.False(result);
        }
    }
