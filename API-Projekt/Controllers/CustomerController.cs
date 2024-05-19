using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomer _customerRepo;
        private IAppointment _appointmentRepo;

        public CustomerController(ICustomer customerRepo, IAppointment appointmentRepo)
        {
            _customerRepo = customerRepo;
            _appointmentRepo = appointmentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _customerRepo.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
        {
            var createdCustomer = await _customerRepo.AddAsync(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            await _customerRepo.UpdateAsync(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepo.DeleteAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetCustomerAppointments(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.Appointments);
        }

        [HttpGet("{id}/appointments/week/{startOfWeek}")]
        public async Task<ActionResult<int>> GetCustomerAppointmentCountForWeek(int id, DateTime startOfWeek)
        {
            var count = await _customerRepo.GetCustomerAppointmentCountWeekAsync(id, startOfWeek);
            return Ok(count);
        }

        [HttpGet("appointments/current-week")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithAppointmentsForWeek()
        {
            var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday); // Assuming an extension method StartOfWeek
            var customers = await _customerRepo.GetCustomersWithAppointmentWeekAsync(startOfWeek);
            return Ok(customers);
        }

        [HttpGet("sorted-filtered")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersSortedAndFiltered(string sortField, string sortOrder, string filterField, string filterValue)
        {
            var customers = await _customerRepo.GetCustomersSortedAndFilteredAsync(sortField, sortOrder, filterField, filterValue);
            return Ok(customers);
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}