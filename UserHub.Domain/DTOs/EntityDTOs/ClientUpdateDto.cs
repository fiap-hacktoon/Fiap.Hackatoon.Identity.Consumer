﻿using FIAP.TechChallenge.UserHub.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.UserHub.Domain.DTOs.EntityDTOs
{
    [ExcludeFromCodeCoverage]
    public class ClientUpdateDto
    {
        [Required(ErrorMessage = "O campo role é obrigatório")]
        public TypeRole TypeRole { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo document é obrigatório")]
        public string Document { get; set; }
        public DateTime Birth { get; set; }
    }
}
