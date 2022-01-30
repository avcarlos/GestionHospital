namespace GestionHospital.Model.Shared
{
    public class DetalleCatalogo
    {
        public int IdCatalogo { get; set; }

        public int IdDetalleCatalogo { get; set; }

        public string Nombre { get; set; }

        public string Codigo { get; set; }

        public int? Parametro1 { get; set; }

        public string Parametro2 { get; set; }

        public bool Estado { get; set; }
    }
}