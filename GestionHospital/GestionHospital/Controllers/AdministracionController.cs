using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
using GestionHospital.Models.Administracion;
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
    public class AdministracionController : Controller
    {
        #region Medico

        [AuthorizeUser(idOperacion: 1)]
        public ActionResult PersonalMedico()
        {
            ViewBag.Title = "Gestión del personal médico";

            AdministracionCore objAdministracion = new AdministracionCore();

            PersonalMedicoView vistaPersonal = new PersonalMedicoView();

            try
            {
                vistaPersonal.FechaActual = DateTime.Now;

                vistaPersonal.ListaTiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);
                vistaPersonal.ListaGeneros = objAdministracion.ConsultarDetallesCatalogo(1);
                vistaPersonal.ListaEspecialidades = objAdministracion.ConsultarEspecialidades();
                vistaPersonal.ListaCiudades = objAdministracion.ConsultarDetallesCatalogo(5);
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_PersonalMedico", vistaPersonal);
        }

        public JsonResult ConsultarMedico(int idTipoIdentificacion, string identificacion)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                List<Especialidad> especialidades = new List<Especialidad>();

                var medico = objAdministracion.ConsultarMedico(idTipoIdentificacion, identificacion);

                if (medico != null)
                    especialidades = medico.Especialidades;

                Session["Especialidades-" + identificacion] = especialidades != null ? especialidades : new List<Especialidad>();

                if (medico != null)
                {
                    return Json(medico);
                }
                else
                {
                    var persona = objAdministracion.ConsultarPersona(idTipoIdentificacion, identificacion);

                    if (persona != null)
                    {
                        return Json(persona);
                    }
                }

                return Json("");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarEspecialidadesMedico([DataSourceRequest] DataSourceRequest request, string identificacion)
        {
            try
            {
                var especialidades = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + identificacion];

                if (especialidades == null)
                    especialidades = new List<Especialidad>();

                return Json(especialidades.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ActualizarEspecialidadMedico(Especialidad especialidad, string identificacion)
        {
            try
            {
                var especialidadesMedico = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + identificacion];

                if (especialidadesMedico == null)
                    especialidadesMedico = new List<Especialidad>();

                if (especialidadesMedico.Exists(e => e.IdEspecialidad == especialidad.IdEspecialidad))
                    throw new Exception(string.Format("Ya se encuentra asignada la especialidad {0}", especialidadesMedico.FirstOrDefault(e => e.IdEspecialidad == especialidad.IdEspecialidad).Nombre));

                var especialidadAnterior = especialidadesMedico.FirstOrDefault(e => e.IdEspecialidadMedico == especialidad.IdEspecialidadMedico);

                especialidadesMedico.Remove(especialidadAnterior);
                especialidadesMedico.Add(especialidad);

                Session["Especialidades-" + identificacion] = especialidadesMedico;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult GuardarEspecialidadMedico(Especialidad especialidad, string identificacion)
        {
            try
            {
                var especialidadesMedico = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + identificacion];

                if (especialidadesMedico == null)
                    especialidadesMedico = new List<Especialidad>();

                if (especialidadesMedico.Exists(e => e.IdEspecialidad == especialidad.IdEspecialidad))
                    throw new Exception(string.Format("Ya se encuentra asignada la especialidad {0}", especialidadesMedico.FirstOrDefault(e => e.IdEspecialidad == especialidad.IdEspecialidad).Nombre));

                if (especialidadesMedico.Count() > 0)
                    especialidad.IdEspecialidadMedico = especialidadesMedico.Min(e => e.IdEspecialidadMedico) < 0 ? especialidadesMedico.Min(e => e.IdEspecialidadMedico) - 1 : -1;
                else
                    especialidad.IdEspecialidadMedico = -1;

                especialidadesMedico.Add(especialidad);

                Session["Especialidades-" + identificacion] = especialidadesMedico;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult EliminarEspecialidadMedica(Especialidad especialidad, string identificacion)
        {
            try
            {
                var especialidadesMedico = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + identificacion];

                if (especialidadesMedico == null)
                    especialidadesMedico = new List<Especialidad>();

                var especialidadEliminar = especialidadesMedico.FirstOrDefault(e => e.IdEspecialidadMedico == especialidad.IdEspecialidadMedico);

                especialidadesMedico.Remove(especialidadEliminar);

                Session["Especialidades-" + identificacion] = especialidadesMedico;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarMedico(PersonalMedicoView vistaPersonal)
        {
            ViewBag.Title = "Gestión del personal médico";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);
                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);
                especialidades = objAdministracion.ConsultarEspecialidades();

                if (ModelState.IsValid)
                {
                    var especialidadesMedico = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + vistaPersonal.Identificacion];

                    Medico medico = new Medico()
                    {
                        IdPersona = vistaPersonal.IdPersona,
                        IdTipoIdentificacion = vistaPersonal.IdTipoIdentificacion,
                        Identificacion = vistaPersonal.Identificacion,
                        Nombres = vistaPersonal.Nombres,
                        Apellidos = vistaPersonal.Apellidos,
                        FechaNacimiento = vistaPersonal.FechaNacimiento,
                        Telefono = vistaPersonal.Telefono,
                        Celular = vistaPersonal.Celular,
                        Email = vistaPersonal.Email,
                        IdGenero = vistaPersonal.IdGenero,
                        Direccion = vistaPersonal.Direccion,
                        IdCiudad = vistaPersonal.IdCiudad,
                        Especialidades = especialidadesMedico
                    };

                    objAdministracion.GuardarMedico(medico);

                    vistaPersonal = new PersonalMedicoView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Médico Guardado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPersonal.FechaActual = fechaActual;
            vistaPersonal.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPersonal.ListaGeneros = generos;
            vistaPersonal.ListaEspecialidades = especialidades;
            vistaPersonal.ListaCiudades = ciudades;

            return View("_PersonalMedico", vistaPersonal);
        }

        public ActionResult ActualizarMedico(PersonalMedicoView vistaPersonal)
        {
            ViewBag.Title = "Gestión del personal médico";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);
                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);
                especialidades = objAdministracion.ConsultarEspecialidades();

                if (ModelState.IsValid)
                {
                    var especialidadesMedico = (List<Especialidad>)System.Web.HttpContext.Current.Session["Especialidades-" + vistaPersonal.Identificacion];

                    Medico medico = new Medico()
                    {
                        IdPersona = vistaPersonal.IdPersona,
                        IdTipoIdentificacion = vistaPersonal.IdTipoIdentificacion,
                        Identificacion = vistaPersonal.Identificacion,
                        Nombres = vistaPersonal.Nombres,
                        Apellidos = vistaPersonal.Apellidos,
                        FechaNacimiento = vistaPersonal.FechaNacimiento,
                        Telefono = vistaPersonal.Telefono,
                        Celular = vistaPersonal.Celular,
                        Email = vistaPersonal.Email,
                        IdGenero = vistaPersonal.IdGenero,
                        Direccion = vistaPersonal.Direccion,
                        IdCiudad = vistaPersonal.IdCiudad,
                        EstadoTipo = vistaPersonal.Estado == 1,
                        Especialidades = especialidadesMedico
                    };

                    objAdministracion.ActualizarMedico(medico);

                    vistaPersonal = new PersonalMedicoView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Médico Actualizado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPersonal.FechaActual = fechaActual;
            vistaPersonal.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPersonal.ListaGeneros = generos;
            vistaPersonal.ListaEspecialidades = especialidades;
            vistaPersonal.ListaCiudades = ciudades;

            return View("_PersonalMedico", vistaPersonal);
        }

        public ActionResult EliminarMedico(PersonalMedicoView vistaPersonal)
        {
            ViewBag.Title = "Gestión del personal médico";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();
            List<Especialidad> especialidades = new List<Especialidad>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);
                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);
                especialidades = objAdministracion.ConsultarEspecialidades();

                if (ModelState.IsValid)
                {
                    Medico medico = new Medico()
                    {
                        IdPersona = vistaPersonal.IdPersona
                    };

                    objAdministracion.EliminarMedico(medico);

                    vistaPersonal = new PersonalMedicoView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Médico Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPersonal.FechaActual = fechaActual;
            vistaPersonal.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPersonal.ListaGeneros = generos;
            vistaPersonal.ListaEspecialidades = especialidades;
            vistaPersonal.ListaCiudades = ciudades;

            return View("_PersonalMedico", vistaPersonal);
        }

        #endregion

        #region Especialidades

        [AuthorizeUser(idOperacion: 11)]
        public ActionResult Especialidades()
        {
            ViewBag.Title = "Gestión de especialidades";
            ViewBag.Message = "En implementación.";

            return View("Index");
        }

        #endregion

        #region Paciente

        [AuthorizeUser(idOperacion: 21)]
        public ActionResult Paciente()
        {
            ViewBag.Title = "Gestión de pacientes";

            AdministracionCore objAdministracion = new AdministracionCore();

            PacienteView vistaPaciente = new PacienteView();

            try
            {
                vistaPaciente.FechaActual = DateTime.Now;

                var tipoIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tipoIdentificaciones != null)
                    tipoIdentificaciones = tipoIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                vistaPaciente.ListaTiposIdentificaciones = tipoIdentificaciones;
                vistaPaciente.ListaGeneros = objAdministracion.ConsultarDetallesCatalogo(1);
                vistaPaciente.ListaCiudades = objAdministracion.ConsultarDetallesCatalogo(5);
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }
            
            return View("_Paciente", vistaPaciente);
        }

        public JsonResult ConsultarPaciente(int idTipoIdentificacion, string identificacion)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var paciente = objAdministracion.ConsultarPaciente(idTipoIdentificacion, identificacion);

                if (paciente != null)
                {
                    return Json(paciente);
                }
                else
                {
                    var persona = objAdministracion.ConsultarPersona(idTipoIdentificacion, identificacion);

                    if (persona != null)
                    {
                        return Json(persona);
                    }
                }

                return Json("");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarPaciente(PacienteView vistaPaciente)
        {
            ViewBag.Title = "Gestión de pacientes";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);

                if (ModelState.IsValid)
                {
                    Paciente paciente = new Paciente()
                    {
                        IdPersona = vistaPaciente.IdPersona,
                        IdTipoIdentificacion = vistaPaciente.IdTipoIdentificacion,
                        Identificacion = vistaPaciente.Identificacion,
                        Nombres = vistaPaciente.Nombres,
                        Apellidos = vistaPaciente.Apellidos,
                        FechaNacimiento = vistaPaciente.FechaNacimiento,
                        Telefono = vistaPaciente.Telefono,
                        Celular = vistaPaciente.Celular,
                        Email = vistaPaciente.Email,
                        IdGenero = vistaPaciente.IdGenero,
                        Direccion = vistaPaciente.Direccion,
                        IdCiudad = vistaPaciente.IdCiudad
                    };

                    objAdministracion.GuardarPaciente(paciente);

                    vistaPaciente = new PacienteView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Paciente Guardado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPaciente.FechaActual = fechaActual;
            vistaPaciente.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPaciente.ListaGeneros = generos;
            vistaPaciente.ListaCiudades = ciudades;

            return View("_Paciente", vistaPaciente);
        }

        public ActionResult ActualizarPaciente(PacienteView vistaPaciente)
        {
            ViewBag.Title = "Gestión de pacientes";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);

                if (ModelState.IsValid)
                {
                    Paciente paciente = new Paciente()
                    {
                        IdPersona = vistaPaciente.IdPersona,
                        IdTipoIdentificacion = vistaPaciente.IdTipoIdentificacion,
                        Identificacion = vistaPaciente.Identificacion,
                        Nombres = vistaPaciente.Nombres,
                        Apellidos = vistaPaciente.Apellidos,
                        FechaNacimiento = vistaPaciente.FechaNacimiento,
                        Telefono = vistaPaciente.Telefono,
                        Celular = vistaPaciente.Celular,
                        Email = vistaPaciente.Email,
                        IdGenero = vistaPaciente.IdGenero,
                        Direccion = vistaPaciente.Direccion,
                        IdCiudad = vistaPaciente.IdCiudad,
                        EstadoTipo = vistaPaciente.Estado == 1
                    };

                    objAdministracion.ActualizarPaciente(paciente);

                    vistaPaciente = new PacienteView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Paciente Actualizado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPaciente.FechaActual = fechaActual;
            vistaPaciente.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPaciente.ListaGeneros = generos;
            vistaPaciente.ListaCiudades = ciudades;

            return View("_Paciente", vistaPaciente);
        }

        public ActionResult EliminarPaciente(PacienteView vistaPaciente)
        {
            ViewBag.Title = "Gestión de pacientes";

            AdministracionCore objAdministracion = new AdministracionCore();

            DateTime fechaActual = DateTime.Now;
            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();
            List<DetalleCatalogo> generos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> ciudades = new List<DetalleCatalogo>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                generos = objAdministracion.ConsultarDetallesCatalogo(1);
                ciudades = objAdministracion.ConsultarDetallesCatalogo(5);

                if (ModelState.IsValid)
                {
                    Paciente paciente = new Paciente()
                    {
                        IdPersona = vistaPaciente.IdPersona
                    };

                    objAdministracion.EliminarPaciente(paciente);

                    vistaPaciente = new PacienteView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Paciente Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPaciente.FechaActual = fechaActual;
            vistaPaciente.ListaTiposIdentificaciones = tiposIdentificaciones;
            vistaPaciente.ListaGeneros = generos;
            vistaPaciente.ListaCiudades = ciudades;

            return View("_Paciente", vistaPaciente);
        }

        #endregion

        #region Paciente Linea

        [AuthorizeUser(idOperacion: 31)]
        public ActionResult PacienteLinea()
        {
            ViewBag.Title = "Gestión de registro en línea";

            PacienteLineaView vistaPaciente = new PacienteLineaView();

            return View("_PacienteLinea", vistaPaciente);
        }

        public ActionResult GuardarPacienteLinea(PacienteLineaView vistaPaciente)
        {
            ViewBag.Title = "Gestión de registro en línea";

            AdministracionCore objAdministracion = new AdministracionCore();

            if (ModelState.IsValid)
            {
                Persona persona = new Persona()
                {
                    IdTipoIdentificacion = 11,
                    Identificacion = vistaPaciente.Identificacion,
                    Nombres = vistaPaciente.Nombres,
                    Apellidos = vistaPaciente.Apellidos,
                    Email = vistaPaciente.Email
                };

                if (vistaPaciente.IdPersona != 0)
                    objAdministracion.ActualizarPersona(persona);
                else
                    objAdministracion.GuardarPersona(persona);
            }

            return View("_PacienteLinea", vistaPaciente);
        }

        #endregion

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