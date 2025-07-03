using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class ClientUpdateConsumer : IConsumer<ClientUpdateEvent>
    {
        private readonly IClientApplication _clientService;

        public ClientUpdateConsumer (IClientApplication clientService)
        {
            _clientService = clientService;
        }

        public async Task Consume(ConsumeContext<ClientUpdateEvent> context)
        {        
            await _clientService.UpdateClientAsync(context.Message);
        }
    }
}
