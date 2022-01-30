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
        #region Roles Seguridad

        [AuthorizeUser(idOperacion: 901)]
        public ActionResult Roles()
        {
            ViewBag.Title = "Gestión de roles";

            SeguridadCore objSeguridad = new SeguridadCore();

            RolesSeguridadView vistaRoles = new RolesSeguridadView();

            try
            {
                vistaRoles.IdTemp = Guid.NewGuid().ToString();
                vistaRoles.ListaTransacciones = objSeguridad.ConsultarTransacciones();
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_RolesSeguridad", vistaRoles);
        }

        public JsonResult CargarGridRolesSeguridad([DataSourceRequest] DataSourceRequest request)
        {
            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                var roles = objSeguridad.ConsultarRolesSeguridad(true);

                if (roles == null)
                    roles = new List<RolSeguridad>();
                else
                {
                    roles = roles.OrderBy(e => e.IdRolSeguridad).ToList();
                }

                return Json(roles.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarTransaccionesRolSeguridad([DataSourceRequest] DataSourceRequest request, int idRolSeguridad, string idTemp)
        {
            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                List<Transaccion> transacciones;

                if (idRolSeguridad > 0)
                {
                    transacciones = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idRolSeguridad.ToString()];

                    if (transacciones == null)
                    {
                        transacciones = objSeguridad.ConsultarTransaccionesRolSeguridad(idRolSeguridad);

                        Session["Transacciones-" + idRolSeguridad.ToString()] = transacciones;
                    }
                }
                else
                {
                    transacciones = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idTemp];

                    if (transacciones == null)
                    {
                        transacciones = new List<Transaccion>();

                        Session["Transacciones-" + idTemp] = transacciones;
                    }
                }

                return Json(transacciones.OrderBy(t => t.IdTransaccion).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ActualizarTransaccionRolSeguridad(Transaccion transaccion, int idRolSeguridad, string idTemp)
        {
            try
            {
                List<Transaccion> transaccionesRol;

                if (idRolSeguridad > 0)
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idRolSeguridad.ToString()];
                else
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idTemp];

                if (transaccionesRol == null)
                    transaccionesRol = new List<Transaccion>();

                if (transaccionesRol.Exists(e => e.IdTransaccion == transaccion.IdTransaccion))
                    throw new Exception(string.Format("Ya se encuentra asignada la transacción {0}", transaccionesRol.FirstOrDefault(e => e.IdTransaccion == transaccion.IdTransaccion).Nombre));

                var transaccionAnterior = transaccionesRol.FirstOrDefault(e => e.IdTransaccionRolSeguridad == transaccion.IdTransaccionRolSeguridad);

                transaccionesRol.Remove(transaccionAnterior);
                transaccionesRol.Add(transaccion);

                Session["Transacciones-" + idTemp] = transaccionesRol;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult GuardarTransaccionRolSeguridad(Transaccion transaccion, int idRolSeguridad, string idTemp)
        {
            try
            {
                List<Transaccion> transaccionesRol;

                if (idRolSeguridad > 0)
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idRolSeguridad.ToString()];
                else
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idTemp];

                if (transaccionesRol == null)
                    transaccionesRol = new List<Transaccion>();

                if (transaccionesRol.Exists(e => e.IdTransaccion == transaccion.IdTransaccion))
                    throw new Exception(string.Format("Ya se encuentra asignada la transacción {0}", transaccionesRol.FirstOrDefault(e => e.IdTransaccion == transaccion.IdTransaccion).Nombre));

                if (transaccionesRol.Count() > 0)
                    transaccion.IdTransaccionRolSeguridad = transaccionesRol.Min(e => e.IdTransaccionRolSeguridad) < 0 ? transaccionesRol.Min(e => e.IdTransaccionRolSeguridad) - 1 : -1;
                else
                    transaccion.IdTransaccionRolSeguridad = -1;

                transaccionesRol.Add(transaccion);

                Session["Transacciones-" + idTemp] = transaccionesRol;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult EliminarTransaccionRolSeguridad(Transaccion transaccion, int idRolSeguridad, string idTemp)
        {
            try
            {
                List<Transaccion> transaccionesRol;

                if (idRolSeguridad > 0)
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idRolSeguridad.ToString()];
                else
                    transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + idTemp];

                if (transaccionesRol == null)
                    transaccionesRol = new List<Transaccion>();

                var transaccionesEliminar = transaccionesRol.FirstOrDefault(e => e.IdTransaccionRolSeguridad == transaccion.IdTransaccionRolSeguridad);

                transaccionesRol.Remove(transaccionesEliminar);

                Session["Transacciones-" + idTemp] = transaccionesRol;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarRolSeguridad(RolesSeguridadView vistaRol)
        {
            ViewBag.Title = "Gestión de roles";

            SeguridadCore objSeguridad = new SeguridadCore();

            List<Transaccion> transacciones = new List<Transaccion>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                transacciones = objSeguridad.ConsultarTransacciones();

                if (ModelState.IsValid)
                {
                    var transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + vistaRol.IdTemp];

                    RolSeguridad rol = new RolSeguridad()
                    {
                        Nombre = vistaRol.NombreRol,
                        Descripcion = vistaRol.DescripcionRol,
                        Transacciones = transaccionesRol
                    };

                    objSeguridad.GuardarRol(rol, usuario);

                    string idTemp = vistaRol.IdTemp;

                    vistaRol = new RolesSeguridadView() { IdTemp = idTemp };

                    Session["Transacciones-" + idTemp] = null;

                    ModelState.Clear();
                }

                ViewBag.Message = "Rol Guardado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaRol.ListaTransacciones = transacciones;

            return View("_RolesSeguridad", vistaRol);
        }

        public ActionResult ActualizarRolSeguridad(RolesSeguridadView vistaRol)
        {
            ViewBag.Title = "Gestión de roles";

            SeguridadCore objSeguridad = new SeguridadCore();

            List<Transaccion> transacciones = new List<Transaccion>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                transacciones = objSeguridad.ConsultarTransacciones();

                if (ModelState.IsValid)
                {
                    var transaccionesRol = (List<Transaccion>)System.Web.HttpContext.Current.Session["Transacciones-" + vistaRol.IdRolSeguridad.ToString()];

                    RolSeguridad rol = new RolSeguridad()
                    {
                        IdRolSeguridad = vistaRol.IdRolSeguridad,
                        Nombre = vistaRol.NombreRol,
                        Descripcion = vistaRol.DescripcionRol,
                        Estado = vistaRol.EstadoOriginalRol ? true : vistaRol.EstadoRol == 1,
                        Transacciones = transaccionesRol
                    };

                    objSeguridad.ActualizarRol(rol, usuario);

                    string idTemp = vistaRol.IdTemp;

                    vistaRol = new RolesSeguridadView() { IdTemp = idTemp };

                    Session["Transacciones-" + rol.IdRolSeguridad.ToString()] = null;

                    ModelState.Clear();
                }

                ViewBag.Message = "Rol Actualizado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaRol.ListaTransacciones = transacciones;

            return View("_RolesSeguridad", vistaRol);
        }

        public ActionResult EliminarRolSeguridad(RolesSeguridadView vistaRol)
        {
            ViewBag.Title = "Gestión de roles";

            SeguridadCore objSeguridad = new SeguridadCore();

            List<Transaccion> transacciones = new List<Transaccion>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                transacciones = objSeguridad.ConsultarTransacciones();

                if (ModelState.IsValid)
                {
                    RolSeguridad rol = new RolSeguridad()
                    {
                        IdRolSeguridad = vistaRol.IdRolSeguridad
                    };

                    objSeguridad.EliminarRolSeguridad(rol, usuario);

                    string idTemp = vistaRol.IdTemp;

                    vistaRol = new RolesSeguridadView() { IdTemp = idTemp };

                    Session["Transacciones-" + rol.IdRolSeguridad.ToString()] = null;

                    ModelState.Clear();
                }

                ViewBag.Message = "Rol Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaRol.ListaTransacciones = transacciones;

            return View("_RolesSeguridad", vistaRol);
        }

        #endregion

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