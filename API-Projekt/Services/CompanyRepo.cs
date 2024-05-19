using Microsoft.EntityFrameworkCore;
using Models;
using Projekt_Avancerad_.Net_Bokning.Data;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class CompanyRepo : ICompany
    {
        private AppDbContext _context;

        public CompanyRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            _context.companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> DeleteCompanyAsync(int id)
        {
            var company = await _context.companies.FindAsync(id);
            if (company != null)
            {
                _context.companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            return company;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.companies.ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await _context.companies.FindAsync(id);
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            _context.companies.Update(company);
            await _context.SaveChangesAsync();
            return company;
        }
    }
}
