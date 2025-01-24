using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoctorAvailability.Services;
using Shared.Entities;
using Shared.Repositories;
using Shared.UnitOfWork;
using static Shared.Enums.SharedEnums;

public class DoctorAvailabilityServiceTests
    {
    private readonly Mock<IRepository<AvailabilitySlot>> _mockAvailabilitySlotRepository;
    private readonly Mock<IRepository<Appointments>> _mockAppointmentRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DoctorAvailabilityService _service;

    public DoctorAvailabilityServiceTests()
        {
        _mockAvailabilitySlotRepository = new Mock<IRepository<AvailabilitySlot>>();
        _mockAppointmentRepository = new Mock<IRepository<Appointments>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _service = new DoctorAvailabilityService(
            _mockUnitOfWork.Object,
            _mockAvailabilitySlotRepository.Object,
            _mockAppointmentRepository.Object
        );

        }
    [Fact]
    public async Task AddSlots_ShouldAddSlotWhenAvailable()
        {
        // Arrange
        var slot = new AvailabilitySlot
            {
            Id = Guid.NewGuid(),
            Time = DateTime.Now.AddHours(1),
            DoctorId = Guid.NewGuid()
            };

        _mockAvailabilitySlotRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<AvailabilitySlot>());

        // Act
        var result = await _service.AddSlots(slot);

        // Assert
        Assert.Equal(1, result);
        _mockAvailabilitySlotRepository.Verify(repo => repo.AddAsync(slot), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
        }
    [Fact]
    public async Task AddSlots_ShouldReturnNegativeOneWhenSlotUnavailable()
        {
        // Arrange
        var slot = new AvailabilitySlot
            {
            Id = Guid.NewGuid(),
            Time = DateTime.Now.AddHours(1),
            DoctorId = Guid.NewGuid()
            };

        _mockAvailabilitySlotRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<AvailabilitySlot>
            {
            new AvailabilitySlot
            {
                Time = slot.Time,
                DoctorId = slot.DoctorId
            }
            });

        // Act
        var result = await _service.AddSlots(slot);

        // Assert
        Assert.Equal(-1, result);
        _mockAvailabilitySlotRepository.Verify(repo => repo.AddAsync(It.IsAny<AvailabilitySlot>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
        }
    [Fact]
    public async Task CheckSlotsAvailability_ShouldReturnTrueWhenSlotAvailable()
        {
        // Arrange
        var slot = new AvailabilitySlot
            {
            Time = DateTime.Now.AddHours(1),
            DoctorId = Guid.NewGuid()
            };

        _mockAvailabilitySlotRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<AvailabilitySlot>());

        // Act
        var result = await _service.CheckSlotsAvailability(slot);

        // Assert
        Assert.True(result);
        }
    [Fact]
    public async Task GetUpComingAppointments_ShouldReturnOnlyUpcomingAppointments()
        {
        // Arrange
        var appointments = new List<Appointments>
    {
        new Appointments { StatusId = (int)AppointmentEnum.UpComing },
        new Appointments { StatusId = (int)AppointmentEnum.Completed }
    };

        _mockAppointmentRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(appointments);

        // Act
        var result = await _service.GetUpComingAppointments();

        // Assert
        Assert.Single(result);
        Assert.All(result, appt => Assert.Equal((int)AppointmentEnum.UpComing, appt.StatusId));
        }
    [Fact]
    public async Task UpdateAppointmentStatus_ShouldUpdateStatusForExistingAppointment()
        {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointments { Id = appointmentId, StatusId = (int)AppointmentEnum.UpComing };
        var updatedAppointment = new Appointments { Id = appointmentId, StatusId = (int)AppointmentEnum.Completed };

        _mockAppointmentRepository
            .Setup(repo => repo.GetByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        // Act
        var result = await _service.UpdateAppointmentStatus(updatedAppointment);

        // Assert
        Assert.True(result);
        _mockAppointmentRepository.Verify(repo => repo.Update(It.Is<Appointments>(a => a.StatusId == updatedAppointment.StatusId)), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
        }
    [Fact]
    public async Task UpdateAppointmentStatus_ShouldReturnFalseForNonExistentAppointment()
        {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointments { Id = appointmentId, StatusId = (int)AppointmentEnum.Completed };

        _mockAppointmentRepository
            .Setup(repo => repo.GetByIdAsync(appointmentId))
            .ReturnsAsync((Appointments)null);

        // Act
        var result = await _service.UpdateAppointmentStatus(appointment);

        // Assert
        Assert.False(result);
        _mockAppointmentRepository.Verify(repo => repo.Update(It.IsAny<Appointments>()), Times.Never);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(CancellationToken.None), Times.Never);
        }
    }
