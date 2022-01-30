using GestionHospital.Model.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Seguridad
{
    public class RolesSeguridadView
    {
        public int IdRolSeguridad { get; set; }

        [Required]
        public string NombreRol { get; set; }

        public string DescripcionRol { get; set; }

        [Required]
        public int EstadoRol { get; set; }

        public bool EstadoOriginalRol { get; set; }

        public string IdTemp { get; set; }

        public List<Transaccion> ListaTransacciones { get; set; }
    }
}