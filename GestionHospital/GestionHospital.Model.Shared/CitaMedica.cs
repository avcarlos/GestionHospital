using System;

namespace GestionHospital.Model.Shared
{
    public class CitaMedica
    {
        public int IdCita { get; set; }

        public int IdPaciente { get; set; }

        public string NombrePaciente { get; set; }

        public int EdadPaciente { get; set; }

        public int IdEspecialidadMedico { get; set; }

        public int IdEspecialidad { get; set; }

        public string NombreEspecialidad { get; set; }

        public int IdMedico { get; set; }

        public string NombreMedico { get; set; }

        public DateTime Fecha { get; set; }

        public int IdHorario { get; set; }

        public string NombreHorario { get; set; }

        public string Motivo { get; set; }

        public string Diagnostico { get; set; }

        public string Observaciones { get; set; }

        public string Examenes { get; set; }

        public string Receta { get; set; }

        public DateTime? FechaProximoControl { get; set; }

        public int IdEstado { get; set; }
    }
}