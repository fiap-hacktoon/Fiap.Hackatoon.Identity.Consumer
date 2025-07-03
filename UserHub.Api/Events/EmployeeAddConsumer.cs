using Fiap.Hackatoon.Shared.Dto;
using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class EmployeeAddConsumer : IConsumer<EmployeeCreateEvent>
    {
        private readonly IEmployeeApplication _employeeService;

        public EmployeeAddConsumer(IEmployeeApplication employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<EmployeeCreateEvent> context)
        {           
            await _employeeService.AddEmployeeAsync(context.Message);
        }
    }
}
