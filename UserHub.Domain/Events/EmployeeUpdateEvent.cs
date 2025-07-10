using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace Fiap.Hackatoon.Shared.Dto
{
    public class EmployeeUpdateEvent
    {
        public int Id { get; set; }
        public TypeRole TypeRole { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }        
    }
}
