using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class ClientAddConsumer : IConsumer<ClientCreateDto>
    {
        private readonly IClientApplication _clientService;

        public ClientAddConsumer(IClientApplication clientService)
        {
            _clientService = clientService;
        }

        public async Task Consume(ConsumeContext<ClientCreateDto> context)
        {
            //var dto = context.Message;

            //// Exemplo de uso
            //var exists = await _clientService.GetClientByEmailAsync(dto.Email);
            //if (exists != null)
            //    return;

            await _clientService.AddClientAsync(context.Message);
        }
    }
}
