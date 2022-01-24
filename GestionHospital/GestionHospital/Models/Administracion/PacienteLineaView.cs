using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Administracion
{
    public class PacienteLineaView
    {
        public int IdPersona { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public PacienteLineaView()
        {
        }
    }
}