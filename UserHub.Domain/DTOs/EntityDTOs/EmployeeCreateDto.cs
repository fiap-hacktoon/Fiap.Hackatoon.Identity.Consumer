using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs
{
    [ExcludeFromCodeCoverage]
    public class EmployeeCreateDto
    {
        [Required(ErrorMessage = "O campo role é obrigatório")]
        public TypeRole TypeRole { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome não pode ter mais de 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Digite a senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Digite a confirmação da senha")]
        public string ConfirmPassword { get; set; }
    }
}
