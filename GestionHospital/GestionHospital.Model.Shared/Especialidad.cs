using System;

namespace GestionHospital.Model.Shared
{
    public class Especialidad
    {
        public int IdEspecialidad { get; set; }

        public int IdMedico { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int IdEspecialidadMedico { get; set; }

        public bool Estado { get; set; }

        public int IdUsuarioRegistro { get; set; }

        public string NombreUsuarioRegistro { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public string NombreUsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}