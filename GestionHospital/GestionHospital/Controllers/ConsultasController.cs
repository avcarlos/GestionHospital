using ExportExcel.Code;
using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
using GestionHospital.Models.Consultas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Controllers
{
    public class ConsultasController : Controller
    {
        [AuthorizeUser(idOperacion: 201)]
        public ActionResult Estadistica()
        {
            ViewBag.Title = "Proceso de estadísticas en salud";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }


        [HttpGet]
        public FileContentResult ExportToExcel()
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            List<Especialidad> especialidades = objAdministracion.ConsultarEspecialidades();
            string[] columns = { "Nombre", "Descripcion" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(especialidades, "Especialidades", true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Medicos.xlsx");
        }

        #region Reporte Detalle Calificaciones

        [AuthorizeUser(idOperacion: 202)]
        public ActionResult ReporteDetalleCalificaciones()
        {
            ViewBag.Title = "Reporte Detalle Calificaciones";

            ReporteDetallesCalificacionesView vistaReporte = new ReporteDetallesCalificacionesView();

            vistaReporte.FechaDesde = DateTime.Now;
            vistaReporte.FechaHasta = DateTime.Now.AddMonths(1);

            return View("_ReporteDetalleCalificaciones", vistaReporte);
        }

        public ActionResult ConsultarDetallesCalificaciones(ReporteDetallesCalificacionesView vistaReporte)
        {
            ViewBag.Title = "Reporte Detalle Calificaciones";

            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                if (ModelState.IsValid)
                {
                    vistaReporte.DatosReporte = objProcesos.ConsultarCitasCalificadas(vistaReporte.FechaDesde, vistaReporte.FechaHasta);

                    Session["DatosReporteDetallesCalificaciones"] = vistaReporte.DatosReporte;
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_ReporteDetalleCalificaciones", vistaReporte);
        }

        public ActionResult ImprimirReporteDetalleCalificaciones()
        {
            ReporteDetallesCalificacionesView vistaReporte = new ReporteDetallesCalificacionesView();

            var datos = (List<CitaMedica>)System.Web.HttpContext.Current.Session["DatosReporteDetallesCalificaciones"];

            vistaReporte.DatosReporte = datos;

            return new Rotativa.ViewAsPdf("PrintReporteDetalleCalificaciones", vistaReporte);
        }

        #endregion
    }
}