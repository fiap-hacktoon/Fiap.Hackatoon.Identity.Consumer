using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class ClientUpdateConsumer : IConsumer<ClientUpdateDto>
    {
        private readonly IClientApplication _clientService;

        public ClientUpdateConsumer (IClientApplication clientService)
        {
            _clientService = clientService;
        }

        public async Task Consume(ConsumeContext<ClientUpdateDto> context)
        {
            var dto = context.Message;

            // Exemplo de uso
            var exists = await _clientService.GetClientByEmailAsync(dto.Email);
            if (exists is null)
                return;

            await _clientService.UpdateClientAsync(context.Message);
        }
    }
}
