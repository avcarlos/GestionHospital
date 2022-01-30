using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class Medico : Persona
    {
        public string NombreMedico { get; set; }

        public List<Especialidad> Especialidades { get; set; }
    }
}