namespace GestionHospital.Model.Shared
{
    public class Especialidad
    {
        public int IdEspecialidad { get; set; }

        public int IdMedico { get; set; }

        public string Nombre { get; set; }

        public int IdEspecialidadMedico { get; set; }
    }
}