using GestionHospital.Model.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Administracion
{
    public class PacienteLineaView
    {
        public int IdPersona { get; set; }

        [Required]
        public int IdTipoIdentificacion { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int IdTipoPersona { get; set; }

        public List<DetalleCatalogo> ListaTiposIdentificaciones { get; set; }

        public bool EsPaciente { get; set; }

        public PacienteLineaView()
        {
            ListaTiposIdentificaciones = new List<DetalleCatalogo>();
        }
    }
}