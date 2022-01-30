using System;
using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class Persona
    {
        public int IdPersona { get; set; }

        public int IdTipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        public int IdGenero { get; set; }

        public string Direccion { get; set; }

        public int IdCiudad { get; set; }

        public int IdTipo {get; set;}

        public bool Estado { get; set; }

        public bool EstadoTipo { get; set; }

        public List<TipoPersona> TiposPersona { get; set; }
    }
}