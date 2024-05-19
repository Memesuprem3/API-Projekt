using Models;

namespace API_Projekt.Services.Interface
{
    public interface IAppointment
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<IEnumerable<Appointment>> GetAppointmentDayAsync(DateTime date);
        Task<IEnumerable<Appointment>> GetAppointmentWeekAsync(int year, int week);
        Task<IEnumerable<Appointment>> GetAppointmentMonthAsync(int year, int month);
        Task<IEnumerable<Appointment>> GetAppointmentYearAsync(int year);
        Task<Appointment> AddAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentAsync(int id);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<Appointment> DeleteAppointmentAsync(Appointment appointment);
        Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync(int appointmentId);
    }
}
