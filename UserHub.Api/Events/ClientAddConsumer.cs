using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class ClientAddConsumer : IConsumer<ClientCreateEvent>
    {
        private readonly IClientApplication _clientService;

        public ClientAddConsumer(IClientApplication clientService)
        {
            _clientService = clientService;
        }

        public async Task Consume(ConsumeContext<ClientCreateEvent> context)
        {    
            await _clientService.AddClientAsync(context.Message);
        }
    }
}
