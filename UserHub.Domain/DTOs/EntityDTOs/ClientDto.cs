using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs
{
    [ExcludeFromCodeCoverage]
    public class ClientDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Documento é obrigatório")]
        [StringLength(14, ErrorMessage = "Documento inválido")]
        public required string Document { get; set; }
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; }
        
        [DataType(DataType.DateTime)]
        public required DateOnly Birth { get; set; }        
    }
}
