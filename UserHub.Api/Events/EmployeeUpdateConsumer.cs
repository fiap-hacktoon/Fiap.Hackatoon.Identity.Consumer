using FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs;
using FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications;
using MassTransit;

namespace FIAP.TechChallenge.UserHub.Api.Events
{
    public class EmployeeUpdateConsumer : IConsumer<EmployeeUpdateDto>
    {
        private readonly IEmployeeApplication _employeeService;

        public EmployeeUpdateConsumer(IEmployeeApplication employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task Consume(ConsumeContext<EmployeeUpdateDto> context)
        {
            var dto = context.Message;

            // Exemplo de uso
            var exists = await _employeeService.GetEmployeeByEmailAsync(dto.Email);
            if (exists is null)
                return;

            await _employeeService.UpdateEmployeeAsync(context.Message);
        }
    }
}
