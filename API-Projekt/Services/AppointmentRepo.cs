using API_Projekt.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class AppointmentRepo : IAppointment
    {
        private AppDbContext _context;

        public AppointmentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            await AddBookingHistoryAsync(appointment.id, "Created");
            return appointment;
        }


        public async Task<Appointment> DeleteAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            await AddBookingHistoryAsync(appointment.id, "Deleted");
            return appointment;
        }

        public async Task<Appointment> GetAppointmentAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date)
        {
            return await _context.Appointments
                                 .Where(a => a.PlacedApp.Date == date.Date).ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month)
        {
            return await _context.Appointments
                                 .Where(a => a.PlacedApp.Year == year && a.PlacedApp.Month == month)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week)
        {
            var appointments = await _context.Appointments
                                   .Where(a => a.PlacedApp.Year == year).ToListAsync();


            var calendar = CultureInfo.InvariantCulture.Calendar;
            var appointmentsInWeek = appointments
                .Where(a => calendar.GetWeekOfYear(a.PlacedApp, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == week).ToList();

            return appointmentsInWeek;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentYearAsync(int year)
        {
            return await _context.Appointments
                                 .Where(a => a.PlacedApp.Year == year)
                                 .ToListAsync();
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            await AddBookingHistoryAsync(appointment.id, "Updated");
            return appointment;
        }

        public async Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int appointmentId)
        {
            return await _context.BookingHistories
                                 .Where(b => b.AppointmentId == appointmentId)
                                 .ToListAsync();
        }

        private async Task AddBookingHistoryAsync(int appointmentId, string changeType)
        {
            var BookingHistory = new BookingHistory
            {
                AppointmentId = appointmentId,
                ChangedAt = DateTime.Now,
                ChangeType = changeType,
                ChangedBy = "User"
            };

            _context.BookingHistories.Add(BookingHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }
    }
}
