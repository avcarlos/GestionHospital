using System;

namespace GestionHospital.Models.Procesos
{
    public class RecetasView
    {
        public int IdPersona { get; set; }

        public int IdReceta { get; set; }

        public string Identificacion { get; set; }

        public string NombrePaciente { get; set; }

        public string NombreMedico { get; set; }

        public DateTime FechaEmision { get; set; }

        public string ObservacionesReceta { get; set; }

        public string LoginUsuario { get; set; }

        public DateTime FechaGeneracion { get; set; }

        public bool EsPaciente { get; set; }
    }
}