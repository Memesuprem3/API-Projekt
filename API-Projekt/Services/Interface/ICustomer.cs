using Models;

namespace API_Projekt.Services.Interface
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<Customer> DeleteAsync(int id);
        Task<IEnumerable<Customer>> GetCustomersWithAppointmentWeekAsync(DateTime startOfWeek);
        Task<int> GetCustomerAppointmentCountWeekAsync(int customerId, DateTime startOfWeek);
        Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue);
    }
}
