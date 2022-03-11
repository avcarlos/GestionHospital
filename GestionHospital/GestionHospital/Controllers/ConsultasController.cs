using ExportExcel.Code;
using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
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
    }
}