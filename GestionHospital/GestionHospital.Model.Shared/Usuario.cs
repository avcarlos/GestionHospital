namespace GestionHospital.Model.Shared
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string LoginUsuario { get; set; }

        public int IdRolSeguridad { get; set; }

        public int? IdPersona { get; set; }

        public bool Estado { get; set; }
    }
}