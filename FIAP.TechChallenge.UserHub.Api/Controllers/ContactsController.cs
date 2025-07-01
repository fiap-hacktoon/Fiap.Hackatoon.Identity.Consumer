using FIAP.TechChallenge.UserHub.Api.Logging;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.UserHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController(IContactApplication contactService, ILogger<ContactsController> logger) : ControllerBase
    {
        private readonly IContactApplication _contactService = contactService;
        private readonly ILogger<ContactsController> _logger = logger;

        /// <summary>
        /// Método para buscar todos os contatos de forma assíncrona.
        /// </summary>
        /// <returns> Retorna uma lista de contatos no formato Json</returns>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ContactDto>> GetAllContacts()
        {
            _logger.LogInformation("Buscando todos os contatos");
            return await _contactService.GetAllContactsAsync();
        }

        /// <summary>
        /// Método para buscar um contato pelo ID de forma assíncrona.
        /// </summary>
        /// <param name="id"> informar o ID do contato</param>
        /// <returns>Retorna um contato filtrado pelo ID no formato Json</returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ContactDto>> GetContactById(int id)
        {
            _logger.LogInformation($"Buscando contato de ID {id}");
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null)
            {
                _logger.LogWarning($"Contato de ID {id} não encontrado");
                return NotFound();
            }

            _logger.LogInformation($"Contato de ID {id} encontrado");
            return contact;
        }

        /// <summary>
        /// Método para buscar contatos por DDD de forma assíncrona.
        /// </summary>
        /// <param name="areaCode"> informar o DDD do contato</param>
        /// <returns> Retorna uma lista de contatos filtrados pelo DDD no formato Json</returns>
        [HttpGet("ddd/{areaCode}")]
        [Authorize]
        public async Task<IEnumerable<ContactDto>> GetContactsByDDD(string areaCode)
        {
            CustomLogger.Arquivo = true;
            _logger.LogInformation("Buscando contatos pelo DDD {DDD}", areaCode);

            try
            {
                return await _contactService.GetContactsByAreaCodeAsync(areaCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Enumerable.Empty<ContactDto>();
            }
        }
    }
}
