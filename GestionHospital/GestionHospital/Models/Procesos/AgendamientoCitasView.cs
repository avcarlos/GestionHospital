using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Procesos
{
    public class AgendamientoCitasView
    {
        public int IdPersona { get; set; }

        public int IdCita { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string NombrePaciente { get; set; }

        [Required]
        public int IdEspecialidad { get; set; }

        [Required]
        public int IdMedico { get; set; }

        [Required]
        public DateTime FechaCita { get; set; }

        [Required]
        public int IdHoraCita { get; set; }

        [Required]
        public string Motivo { get; set; }

        public bool EsPaciente { get; set; }

        public DateTime FechaActual { get; set; }

        public AgendamientoCitasView()
        {
        }
    }
}