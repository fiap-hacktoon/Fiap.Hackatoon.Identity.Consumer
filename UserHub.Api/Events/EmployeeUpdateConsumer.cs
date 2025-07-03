using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class EmployeeUpdateConsumer : IConsumer<EmployeeUpdateEvent>
    {
        private readonly IEmployeeApplication _employeeService;

        public EmployeeUpdateConsumer(IEmployeeApplication employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<EmployeeUpdateEvent> context)
        {
            await _employeeService.UpdateEmployeeAsync(context.Message);
        }
    }
}
