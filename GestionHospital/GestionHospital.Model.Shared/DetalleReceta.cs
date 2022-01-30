namespace GestionHospital.Model.Shared
{
    public class DetalleReceta
    {
        public int IdDetalleReceta { get; set; }

        public int IdReceta { get; set; }

        public int IdMedicamento { get; set; }

        public string Medicamento { get; set; }

        public string Indicaciones { get; set; }
    }
}