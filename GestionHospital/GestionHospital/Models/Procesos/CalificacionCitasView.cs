using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;

namespace GestionHospital.Models.Procesos
{
    public class CalificacionCitasView
    {
        public int IdCita { get; set; }

        public int IdPaciente { get; set; }

        public string NombrePaciente { get; set; }

        public string NombreMedico { get; set; }

        public string NombreEspecialidad { get; set; }

        public DateTime? FechaCita { get; set; }

        public int IdCalificacion { get; set; }

        public List<DetalleCatalogo> ListaCalificaciones { get; set; }

        public CalificacionCitasView()
        {
            ListaCalificaciones = new List<DetalleCatalogo>();
        }
    }
}