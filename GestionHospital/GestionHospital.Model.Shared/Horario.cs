using System;

namespace GestionHospital.Model.Shared
{
    public class Horario
    {
        public int IdHorario { get; set; }

        public string Nombre { get; set; }

        public TimeSpan HoraInicio { get; set; }

        public TimeSpan HoraFin { get; set; }

        public int IdTipoHorario { get; set; }

        public bool Estado { get; set; }
    }
}