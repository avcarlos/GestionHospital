using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;

namespace GestionHospital.Models.Procesos
{
    public class GestionCitasView
    {
        public int IdMedico { get; set; }

        public string IdentificacionMedico { get; set; }

        public string NombreMedico { get; set; }

        public DateTime FechaConsulta { get; set; }

        public int IdCita { get; set; }

        public string NombreEspecialidad { get; set; }
        
        public DateTime? FechaCita { get; set; }

        public string HoraCita { get; set; }

        public string NombrePaciente { get; set; }

        public string Motivo { get; set; }

        public string Diagnostico { get; set; }

        public string Examenes { get; set; }

        public string Receta { get; set; }

        public int IdEstadoCita { get; set; }

        public DateTime? FechaProximoControl { get; set; }

        public bool EsMedico { get; set; }

        public DateTime FechaActual { get; set; }

        public List<DetalleCatalogo> ListaEstados { get; set; }

        public GestionCitasView()
        {
            ListaEstados = new List<DetalleCatalogo>();
        }
    }
}