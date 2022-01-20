using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class Medico : Persona
    {
        public List<Especialidad> Especialidades { get; set; }
    }
}