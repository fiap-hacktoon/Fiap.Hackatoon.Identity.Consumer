using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Hackatoon.Shared.Dto
{
    [ExcludeFromCodeCoverage]
    public class ClientCreateDto: Event
    {
        [Required(ErrorMessage = "O campo role é obrigatório")]
        public TypeRole TypeRole { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome não pode ter mais de 100 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo document é obrigatório")]
        [StringLength(14, ErrorMessage = "Documento inválido")]
        public string Document { get; set; }

        [Required(ErrorMessage = "Digite a senha")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Digite a confirmação da senha")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "O campo Birth é obrigatório")]
        [DataType(DataType.DateTime)]
        public DateTime Birth { get; set; }
    }
}
