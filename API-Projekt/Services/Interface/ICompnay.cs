using Models;

namespace API_Projekt.Services.Interface
{
    public interface ICompnay
    {
        Task<Company> GetCompanyByIdAsync(int id);
        Task<Company> AddCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<Company> DeleteCompanyAsync(int id);

        Task<IEnumerable<Company>> GetAllCompaniesAsync();
    }
}
