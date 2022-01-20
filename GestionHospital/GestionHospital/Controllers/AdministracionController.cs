using GestionHospital.Filters;
using GestionHospital.Models.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Controllers
{
    public class AdministracionController : Controller
    {
        [AuthorizeUser(idOperacion: 1)]
        public ActionResult PersonalMedico()
        {
            ViewBag.Title = "Gestión del personal médico";

            PersonalMedicoView vistaPersonal = new PersonalMedicoView();

            vistaPersonal.FechaActual = DateTime.Now;

            return View("_PersonalMedico", vistaPersonal);
        }

        [AuthorizeUser(idOperacion: 11)]
        public ActionResult Especialidades()
        {
            ViewBag.Title = "Gestión de especialidades";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        [AuthorizeUser(idOperacion: 21)]
        public ActionResult Paciente()
        {
            ViewBag.Title = "Gestión de pacientes";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        [AuthorizeUser(idOperacion: 31)]
        public ActionResult PacienteLinea()
        {
            ViewBag.Title = "Gestión de registro en línea";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }
    }
}