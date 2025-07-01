using FIAP.TechChallenge.UserHub.Domain.Entities;

namespace FIAP.TechChallenge.UserHub.Domain.Interfaces.Applications
{
    public interface ITokenApplication
    {
        public string GetToken(Employee usuario);
    }
}
