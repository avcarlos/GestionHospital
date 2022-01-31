using System;
using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class Receta
    {
        public int IdReceta { get; set; }

        public int IdCita { get; set; }

        public string Observaciones { get; set; }

        public string NombreMedico { get; set; }

        public string NombrePaciente { get; set; }

        public string NombreEspecialidad { get; set; }

        public DateTime Fecha { get; set; }

        public string LoginUsuario { get; set; }

        public DateTime FechaModificacion { get; set; }

        public List<DetalleReceta> Detalles { get; set; }

        public Receta()
        {
            Detalles = new List<DetalleReceta>();
        }
    }
}