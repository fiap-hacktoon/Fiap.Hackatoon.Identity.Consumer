using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int TypeRole { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Documento é obrigatório")]
        [StringLength(14,ErrorMessage = "Documento inválido")]
        public required string Document { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "Email não pode ter mais de 100 caracteres")]
        public required string Email { get; set; }

        //[Required(ErrorMessage = "Password é obrigatório")]
        //[StringLength(250, ErrorMessage = "Password não pode ter mais de 250 caracteres")]
        //public required string Password { get; set; }
        
        [StringLength(250, ErrorMessage = "Password não pode ter mais de 250 caracteres")]
        public string Password { get; set; }
        public DateOnly Birth { get; set; }
        [DataType(DataType.DateTime)]
        public required DateTime Creation { get; set; }
    }
}