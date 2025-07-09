using FIAP.TechChallenge.UserHub.Domain.Enumerators;

namespace Fiap.Hackatoon.Shared.Dto
{
    public class ClientCreateEvent
    {       
        public TypeRole TypeRole { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string Password { get; set; }    
        public DateOnly Birth { get; set; }
    }
}
