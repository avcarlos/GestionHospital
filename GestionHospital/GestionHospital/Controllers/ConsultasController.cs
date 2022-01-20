using GestionHospital.Filters;
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
    }
}