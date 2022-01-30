using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Seguridad
{
    public class PermisosView
    {
        public int IdTransaccion { get; set; }

        [Required]
        public string NombreTransaccion { get; set; }

        public string DescripcionTransaccion { get; set; }

        [Required]
        public int EstadoTransaccion { get; set; }

        public bool EstadoOriginalTransaccion { get; set; }
    }
}