using System.Collections.Generic;

namespace GestionHospital.Model.Shared
{
    public class RolSeguridad
    {
        public int IdRolSeguridad { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public List<Transaccion> Transacciones { get; set; }

        public RolSeguridad()
        {
            Transacciones = new List<Transaccion>();
        }
    }
}