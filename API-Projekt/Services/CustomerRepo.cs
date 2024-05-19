using API_Projekt.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class CustomerRepo : ICustomer
    {
        private AppDbContext _context;

        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.customer.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> DeleteAsync(int id)
        {
            var customer = await _context.customer.FindAsync(id);
            if (customer != null)
            {
                _context.customer.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _context.customer.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }


        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.customer.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.customer
                                 .Include(c => c.Appointments)
                                 .FirstOrDefaultAsync(c => c.CustomerId == id);
        }


        public async Task<IEnumerable<Customer>> GetCustomersWithAppointmentWeekAsync(DateTime startOfWeek)
        {
            var endOfWeek = startOfWeek.AddDays(7);
            return await _context.customer
                                 .Include(c => c.Appointments)
                                 .Where(c => c.Appointments.Any(a => a.PlacedApp >= startOfWeek &&
                                                                     a.PlacedApp < endOfWeek))
                                 .ToListAsync();
        }


        public async Task<int> GetCustomerAppointmentCountWeekAsync(int customerId, DateTime startOfWeek)
        {
            var endOfWeek = startOfWeek.AddDays(7);
            return await _context.Appointments.Where(a => a.CustomerId == customerId &&
                                                          a.PlacedApp <= startOfWeek &&
                                                          a.PlacedApp < endOfWeek)
                                                          .CountAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersSortedAndFilteredAsync(string sortField, string sortOrder, string filterField, string filterValue)
        {
            IQueryable<Customer> query = _context.customer.Include(c => c.CustomerId);

            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                if (filterField == "Name")
                {
                    query = query.Where(c => c.FristName.Contains(filterValue) || c.LastName.Contains(filterValue));
                }
            }

            if (!string.IsNullOrEmpty(filterField) && !string.IsNullOrEmpty(filterValue))
            {
                if (sortField == "Name" && sortOrder == "asc")
                {
                    query = query.OrderBy(c => c.LastName).ThenBy(c => c.FristName);
                }
                else if (sortField == "Name" && sortOrder == "desc")
                {
                    query = query.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FristName);
                }
            }
            return await query.ToListAsync();
        }


    }
}
