using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs
{
    [ExcludeFromCodeCoverage]
    public class EmployeeDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome não pode ter mais de 50 caracteres")]
        public required string Name { get; set; }
        [StringLength(5, ErrorMessage = "Role não pode ter mais de 5 caracteres")]
        public required TypeRole TypeRole { get; set; }        

        [StringLength(50, ErrorMessage = "Email não pode ter mais de 50 caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
    }
}
