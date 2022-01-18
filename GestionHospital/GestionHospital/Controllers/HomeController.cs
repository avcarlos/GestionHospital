using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GestionHospital.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeUser(idOperacion:1)]
        public ActionResult Index()
        {
            SeguridadCore objSeguridad = new SeguridadCore();

            var usuarios = objSeguridad.ConsultarUsuarios();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetTransacciones()
        {
            var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

            if (usuario != null && usuario.Login.Contains("carlosabalco"))
            {
                var result = new[] {
                    new { Nombre = "Home", UrlTransaccion = UrlHelper.GenerateUrl(null, "Index", "Home", null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false) },
                    new { Nombre = "Acerca de", UrlTransaccion = UrlHelper.GenerateUrl(null, "About", "Home", null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false) },
                    new { Nombre = "Contacto", UrlTransaccion = UrlHelper.GenerateUrl(null, "Contact", "Home", null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false) }
                };

                return Json(result.ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new[] {
                    new { Nombre = "Home", UrlTransaccion = UrlHelper.GenerateUrl(null, "Index", "Home", null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false) },
                    new { Nombre = "Contacto", UrlTransaccion = UrlHelper.GenerateUrl(null, "Contact", "Home", null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false) }
                };

                return Json(result.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}