using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.UserHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController(IClientApplication clientService, ILogger<ClientController> logger) : ControllerBase
    {
        private readonly IClientApplication _clientService = clientService;
        private readonly ILogger<ClientController> _logger = logger;

        /// <summary>
        /// Método para buscar todos os clientes de forma assíncrona.
        /// </summary>
        /// <returns> Retorna uma lista de clientes no formato Json</returns>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ClientDto>> GetAllContacts()
        {
            _logger.LogInformation("Buscando todos os clientes");
            return await _clientService.GetAllClientsAsync();
        }

        /// <summary>
        /// Método para buscar um cliente pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="id"> informar o ID do cliente</param>
        /// <returns>Retorna um cliente filtrado pelo ID no formato Json</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ClientDto>> GetContactById(int id)
        {
            _logger.LogInformation($"Buscando cliente de ID {id}");
            var contact = await _clientService.GetClientByIdAsync(id);
            if (contact == null)
            {
                _logger.LogWarning($"cliente de ID {id} não encontrado");
                return NotFound();
            }

            _logger.LogInformation($"cliente de ID {id} encontrado");
            return contact;
        }        

        /// <summary>
        /// Método para buscar clientes por Email de forma assíncrona.
        /// </summary>
        /// <param name="email"> informar o Email do cliente</param>
        /// <returns> Retorna uma lista de clientes filtrados pelo Email no formato Json</returns>
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<ActionResult<ClientDto>> GetContactByEmailAsync(string email)
        {
            _logger.LogInformation("Buscando clientes pelo Email {Email}", email);

            try
            {
                return await _clientService.GetClientByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
