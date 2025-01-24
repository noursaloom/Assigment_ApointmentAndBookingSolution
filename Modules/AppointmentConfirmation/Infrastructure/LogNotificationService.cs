using AppointmentConfirmation.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentConfirmation.Infrastructure
    {
    public class LogNotificationService : INotificationService
        {
        public void SendConfirmation(string message)
            {
            Console.WriteLine($"Notification Sent: \n{message}");
            }
        }
    }
