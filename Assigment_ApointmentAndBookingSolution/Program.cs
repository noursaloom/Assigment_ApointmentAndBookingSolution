using AppointmentConfirmation.Application.Interfaces;
using AppointmentConfirmation.Application.Services;
using AppointmentConfirmation.Infrastructure;
using DoctorAppointmentManagement.Core.Application.Services;
using DoctorAvailability.Services;
using Microsoft.EntityFrameworkCore;
using Shared.DBContext;
using Shared.Repositories;
using Shared.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ISlotRepository, SlotRepository>(); 
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<INotificationService, LogNotificationService>(); 


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DoctorAvailabilityService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AppointmentManagementService>();
builder.Services.AddScoped<AppointmentConfirmationService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("DoctorAvailability", new Microsoft.OpenApi.Models.OpenApiInfo
        {
        Title = "Doctor Availability API",
        Version = "v1"
        });

    c.SwaggerDoc("AppointmentBooking", new Microsoft.OpenApi.Models.OpenApiInfo
        {
        Title = "Appointment Booking API",
        Version = "v1"
        });

    c.SwaggerDoc("AppointmentConfirmation", new Microsoft.OpenApi.Models.OpenApiInfo
        {
        Title = "Appointment Confirmation API",
        Version = "v1"
        });

    c.SwaggerDoc("DoctorAppointmentManagement", new Microsoft.OpenApi.Models.OpenApiInfo
        {
        Title = "Doctor Appointment Management API",
        Version = "v1"
        });

    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
            {
            return new[] { api.GroupName };
            }

        return new[] { api.ActionDescriptor.RouteValues["controller"] ?? "Default" };
    });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.ActionDescriptor.RouteValues["controller"];
        return string.Equals(docName, groupName, StringComparison.OrdinalIgnoreCase);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/DoctorAvailability/swagger.json", "Doctor Availability API v1");
        c.SwaggerEndpoint("/swagger/AppointmentBooking/swagger.json", "Appointment Booking API v1");
        c.SwaggerEndpoint("/swagger/AppointmentConfirmation/swagger.json", "Appointment Confirmation API v1");
        c.SwaggerEndpoint("/swagger/DoctorAppointmentManagement/swagger.json", "Doctor Appointment Management API v1");

        c.RoutePrefix = "swagger";
    });
    }

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
