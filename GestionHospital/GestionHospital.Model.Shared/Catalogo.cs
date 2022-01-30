namespace GestionHospital.Model.Shared
{
    public class Catalogo
    {
        public int IdCatalogo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public bool Estado { get; set; }

        public bool Administrable { get; set; }
    }
}