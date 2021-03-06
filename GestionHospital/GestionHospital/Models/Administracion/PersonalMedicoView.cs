using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Administracion
{
    public class PersonalMedicoView
    {
        public int IdPersona { get; set; }

        [Required]
        public int IdTipoIdentificacion { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Telefono { get; set; }

        public string Celular { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int IdGenero { get; set; }

        public string Direccion { get; set; }

        public int IdCiudad { get; set; }

        public int IdEspecialidad { get; set; }

        public int Estado { get; set; }

        public int IdTipoPersona { get; set; }

        public List<Especialidad> ListaEspecialidades { get; set; }

        public List<DetalleCatalogo> ListaTiposIdentificaciones { get; set; }

        public List<DetalleCatalogo> ListaGeneros { get; set; }

        public List<DetalleCatalogo> ListaCiudades { get; set; }

        public DateTime FechaActual { get; set; }

        public PersonalMedicoView()
        {
            ListaEspecialidades = new List<Especialidad>();
            ListaTiposIdentificaciones = new List<DetalleCatalogo>();
            ListaGeneros = new List<DetalleCatalogo>();
            ListaCiudades = new List<DetalleCatalogo>();
        }
    }
}