using GestionHospital.Model.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Administracion
{
    public class DetallesCatalogoView
    {
        public int IdDetalleCatalogo { get; set; }

        public int IdCatalogo { get; set; }

        [Required]
        public string NombreDetalle { get; set; }

        public string CodigoDetalle { get; set; }

        [Required]
        public int EstadoDetalle { get; set; }

        public bool EstadoOriginalDetalleCatalogo { get; set; }

        public List<Catalogo> ListaCatalogos { get; set; }
    }
}