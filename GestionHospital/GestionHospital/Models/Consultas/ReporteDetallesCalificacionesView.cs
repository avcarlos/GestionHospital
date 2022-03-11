using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionHospital.Models.Consultas
{
    public class ReporteDetallesCalificacionesView
    {
        [Required]
        public DateTime FechaDesde { get; set; }

        [Required]
        public DateTime FechaHasta { get; set; }

        public List<CitaMedica> DatosReporte { get; set; }

        public ReporteDetallesCalificacionesView()
        {
            DatosReporte = new List<CitaMedica>();
        }
    }
}