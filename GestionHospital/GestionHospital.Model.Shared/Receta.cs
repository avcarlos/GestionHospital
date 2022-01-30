using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class Receta
    {
        public int IdReceta { get; set; }

        public int IdCita { get; set; }

        public string Observaciones { get; set; }

        public List<DetalleReceta> Detalles { get; set; }

        public Receta()
        {
            Detalles = new List<DetalleReceta>();
        }
    }
}