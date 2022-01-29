using System;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Administracion
{
    public class EspecialidadesView
    {
        public int IdEspecialidad { get; set; }

        [Required]
        public string NombreEspecialidad { get; set; }

        public string Descripcion { get; set; }

        public string UsuarioRegistro { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [Required]
        public int Estado { get; set; }

        public bool EstadoOriginalEspecialidad { get; set; }
    }
}