using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public static class EmployeeList
    {
        public static IList<Employee>? Users { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Employee
    {
        public int Id { get; set; }

        public required int TypeRole { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome não pode ter mais de 50 caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [StringLength(50, ErrorMessage = "Email não pode ter mais de 50 caracteres")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        [StringLength(50, ErrorMessage = "Password não pode ter mais de 50 caracteres")]
        public required string Password { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime Creation { get; set; }

        public TypePermission Permission { get; set; }
    }
}