using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
using GestionHospital.Models.Seguridad;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        #region Transacciones

        [AuthorizeUser(idOperacion: 911)]
        public ActionResult Permisos()
        {
            ViewBag.Title = "Gestión de permisos";

            PermisosView vistaPermisos = new PermisosView();

            return View("_Permisos", vistaPermisos);
        }

        public JsonResult CargarGridTransacciones([DataSourceRequest] DataSourceRequest request)
        {
            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                var transacciones = objSeguridad.ConsultarTransacciones(true);

                if (transacciones == null)
                    transacciones = new List<Transaccion>();
                else
                {
                    transacciones = transacciones.OrderBy(e => e.IdTransaccion).ToList();
                }

                return Json(transacciones.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult ActualizarTransaccion(PermisosView vistaPermisos)
        {
            ViewBag.Title = "Gestión de permisos";

            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (ModelState.IsValid)
                {
                    Transaccion transaccion = new Transaccion()
                    {
                        IdTransaccion = vistaPermisos.IdTransaccion,
                        Nombre = vistaPermisos.NombreTransaccion,
                        Descripcion = vistaPermisos.DescripcionTransaccion,
                        Estado = vistaPermisos.EstadoOriginalTransaccion ? true : vistaPermisos.EstadoTransaccion == 1
                    };

                    objSeguridad.ActualizarTransaccion(transaccion, usuario);

                    vistaPermisos = new PermisosView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Transacción Actualizada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_Permisos", vistaPermisos);
        }

        public ActionResult EliminarTransaccion(PermisosView vistaPermisos)
        {
            ViewBag.Title = "Gestión de permisos";

            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (ModelState.IsValid)
                {
                    Transaccion transaccion = new Transaccion()
                    {
                        IdTransaccion = vistaPermisos.IdTransaccion
                    };

                    objSeguridad.EliminarTransaccion(transaccion, usuario);

                    vistaPermisos = new PermisosView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Transacción Eliminada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_Permisos", vistaPermisos);
        }

        #endregion

        [AuthorizeUser(idOperacion: 921)]
        public ActionResult Usuarios()
        {
            ViewBag.Title = "Gestión de usuarios";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        #region Comunes

        private JsonResult RetornarErrorJsonResult(string mensajeError)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Response.StatusDescription = mensajeError.Replace(Environment.NewLine, string.Empty);

            return Json(mensajeError, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}