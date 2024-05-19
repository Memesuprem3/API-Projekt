using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Projekt_Avancerad_.Net_Bokning.DTO;
using Projekt_Models;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using API_Projekt.DTO;
namespace Projekt_Avancerad_.Net_Bokning.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompany _companyRepo;
        private readonly IAppointment _appointmentRepo;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompany companyRepo, IAppointment appointmentRepo, ILogger<CompanyController> logger)
        {
            _companyRepo = companyRepo;
            _appointmentRepo = appointmentRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanies()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var claims = identity.Claims.ToList();
                foreach (var claim in claims)
                {
                    _logger.LogInformation($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }
            }

            var companies = await _companyRepo.GetAllCompaniesAsync();
            var companyDtos = companies.Select(company => new CompanyDTO
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName
            }).ToList();

            return Ok(companyDtos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDTO>> GetCompanyById(int id)
        {
            var company = await _companyRepo.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var companyDto = new CompanyDTO
            {
                CompanyId = company.CompanyId,
                CompanyName = company.CompanyName
            };

            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDTO>> AddCompany(CompanyDTO companyDto)
        {
            var company = new Company
            {
                CompanyName = companyDto.CompanyName
            };

            var createdCompany = await _companyRepo.AddCompanyAsync(company);
            var createdCompanyDto = new CompanyDTO
            {
                CompanyId = createdCompany.CompanyId,
                CompanyName = createdCompany.CompanyName
            };

            return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompanyDto.CompanyId }, createdCompanyDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDTO companyDto)
        {
            if (id != companyDto.CompanyId)
            {
                return BadRequest("Company with that ID not found");
            }

            var company = new Company
            {
                CompanyId = companyDto.CompanyId,
                CompanyName = companyDto.CompanyName
            };

            await _companyRepo.UpdateCompanyAsync(company);
            return Ok("Success");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _companyRepo.DeleteCompanyAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok("Company Deleted");
        }

        [HttpGet("{id}/appointments/month/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByMonth(int id, int year, int month)
        {
            var appointments = await _appointmentRepo.GetAppointmentMonthAsync(year, month);
            var companyAppointments = appointments.Where(a => a.CompanyId == id)
                                                  .Select(a => new AppointmentDTO
                                                  {
                                                      Id = a.id,
                                                      AppointmentDescription = a.AppointDiscription,
                                                      PlacedApp = a.PlacedApp,
                                                      CustomerId = a.CustomerId,
                                                      CompanyId = a.CompanyId
                                                  }).ToList();

            return Ok(companyAppointments);
        }

        [HttpGet("{id}/appointments/week/{year}/{week}")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByWeek(int id, int year, int week)
        {
            var appointments = await _appointmentRepo.GetAppointmentWeekAsync(year, week);
            var companyAppointments = appointments.Where(a => a.CompanyId == id)
                                                  .Select(a => new AppointmentDTO
                                                  {
                                                      Id = a.id,
                                                      AppointmentDescription = a.AppointDiscription,
                                                      PlacedApp = a.PlacedApp,
                                                      CustomerId = a.CustomerId,
                                                      CompanyId = a.CompanyId
                                                  }).ToList();

            return Ok(companyAppointments);
        }
    }
}