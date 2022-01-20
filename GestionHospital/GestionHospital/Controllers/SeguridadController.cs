using GestionHospital.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Controllers
{
    public class SeguridadController : Controller
    {
        [AuthorizeUser(idOperacion: 901)]
        public ActionResult Roles()
        {
            ViewBag.Title = "Gestión de roles";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        [AuthorizeUser(idOperacion: 911)]
        public ActionResult Permisos()
        {
            ViewBag.Title = "Gestión de permisos";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        [AuthorizeUser(idOperacion: 921)]
        public ActionResult Usuarios()
        {
            ViewBag.Title = "Gestión de usuarios";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }
    }
}