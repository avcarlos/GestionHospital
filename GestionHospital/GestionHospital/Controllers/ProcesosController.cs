using GestionHospital.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Controllers
{
    public class ProcesosController : Controller
    {
        [AuthorizeUser(idOperacion: 101)]
        public ActionResult AgendamientoCita()
        {
            ViewBag.Title = "Gestión de citas";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        [AuthorizeUser(idOperacion: 111)]
        public ActionResult AgendaCitas()
        {
            ViewBag.Title = "Visualización de citas por parte del médico";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }
    }
}