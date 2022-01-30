namespace GestionHospital.Model.Shared
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public string Menu { get; set; }

        public int IdTransaccionRolSeguridad { get; set; }
    }
}