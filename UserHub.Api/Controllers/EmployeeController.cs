using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.UserHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeApplication employeeService, ILogger<EmployeeController> logger) : ControllerBase
    {
        private readonly IEmployeeApplication _employeeService = employeeService;
        private readonly ILogger<EmployeeController> _logger = logger;

        /// <summary>
        /// Método para buscar todos os funcionários de forma assíncrona.
        /// </summary>
        /// <returns> Retorna uma lista de funcionários no formato Json</returns>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<EmployeeDto>> GetAllContacts()
        {
            _logger.LogInformation("Buscando todos os funcionários");
            return await _employeeService.GetAllEmployeesAsync();
        }

        /// <summary>
        /// Método para buscar um funcionário pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="id"> informar o ID do funcionário</param>
        /// <returns>Retorna um funcionário filtrado pelo ID no formato Json</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> GetContactById(int id)
        {
            _logger.LogInformation($"Buscando funcionário de ID {id}");
            var contact = await _employeeService.GetEmployeeByIdAsync(id);
            if (contact == null)
            {
                _logger.LogWarning($"funcionário de ID {id} não encontrado");
                return NotFound();
            }

            _logger.LogInformation($"funcionário de ID {id} encontrado");
            return contact;
        }

        /// <summary>
        /// Método para buscar funcionários por Email de forma assíncrona.
        /// </summary>
        /// <param name="email"> informar o Email do funcionário</param>
        /// <returns> Retorna uma lista de funcionários filtrados pelo Email no formato Json</returns>
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<ActionResult<EmployeeDto>> GetContactByEmailAsync(string email)
        {
            _logger.LogInformation("Buscando funcionários pelo Email {Email}", email);

            try
            {
                return await _employeeService.GetEmployeeByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Método para buscar funcionários por Cargo de forma assíncrona.
        /// </summary>
        /// <param name="role"> informar o Cargo do funcionário</param>
        /// <returns> Retorna uma lista de funcionários filtrados por Cargo no formato Json</returns>
        [HttpGet("role/{role}")]
        [Authorize]
        public async Task<IEnumerable<EmployeeDto>> GetContactByRoleAsync(TypeRole role)
        {
            _logger.LogInformation("Buscando funcionários pelo Email {Email}", role);

            try
            {
                return await _employeeService.GetAllEmployeesByRoleAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
