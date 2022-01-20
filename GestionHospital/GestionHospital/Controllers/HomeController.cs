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
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTransacciones()
        {
            List<TransaccionMenu> transaccionesMenu = new List<TransaccionMenu>();

            SeguridadCore objSeguridad = new SeguridadCore();

            var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

            if (usuario != null)
            {
                var transacciones = objSeguridad.ConsultarTransaccionesUsuario(usuario.LoginUsuario.Trim());

                foreach (var tran in transacciones.FindAll(t => !string.IsNullOrEmpty(t.Menu)))
                {
                    var datosMenu = tran.Menu.Split('\\');

                    if (datosMenu.Length == 2)
                    {
                        TransaccionMenu menu = new TransaccionMenu();

                        menu.Nombre = tran.Nombre;
                        menu.UrlTransaccion = UrlHelper.GenerateUrl(null, datosMenu[1], datosMenu[0], null, RouteTable.Routes, System.Web.HttpContext.Current.Request.RequestContext, false);

                        transaccionesMenu.Add(menu);
                    }
                }
            }

            return Json(transaccionesMenu, JsonRequestBehavior.AllowGet);
        }
    }
}