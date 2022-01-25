using System;

namespace GestionHospital.Model.Shared
{
    public class Horario
    {
        public int IdHorario { get; set; }

        public string Nombre { get; set; }

        public DateTime HoraInicio { get; set; }

        public DateTime HoraFin { get; set; }

        public int IdTipoHorario { get; set; }

        public bool Estado { get; set; }
    }
}