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

            EspecialidadesView vistaEspecialidades = new EspecialidadesView();

            return View("_Especialidades", vistaEspecialidades);
        }

        public JsonResult CargarGridEspecialidades([DataSourceRequest] DataSourceRequest request)
        {
            AdministracionCore objAdministracion = new AdministracionCore();
            SeguridadCore objSeguridad = new SeguridadCore();

            try
            {
                var especialidades = objAdministracion.ConsultarEspecialidades(true);

                if (especialidades == null)
                    especialidades = new List<Especialidad>();
                else
                {
                    var usuarios = objSeguridad.ConsultarUsuarios();

                    foreach (var item in especialidades)
                    {
                        if (item.IdUsuarioRegistro != 0)
                            item.NombreUsuarioRegistro = usuarios.FirstOrDefault(u => u.IdUsuario == item.IdUsuarioRegistro).LoginUsuario;

                        if (item.IdUsuarioModificacion != 0)
                            item.NombreUsuarioModificacion = usuarios.FirstOrDefault(u => u.IdUsuario == item.IdUsuarioModificacion).LoginUsuario;
                    }

                    especialidades = especialidades.OrderBy(e => e.IdEspecialidad).ToList();
                }

                return Json(especialidades.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarEspecialidad(EspecialidadesView vistaEspecialidad)
        {
            ViewBag.Title = "Gestión de especialidades";

            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (ModelState.IsValid)
                {
                    var especialidades = objAdministracion.ConsultarEspecialidades(true);

                    if (especialidades.Exists(e => e.Nombre == vistaEspecialidad.NombreEspecialidad))
                        throw new Exception("Ya existe una especilidad con ese nombre.");

                    Especialidad especialidad = new Especialidad()
                    {
                        Nombre = vistaEspecialidad.NombreEspecialidad,
                        Descripcion = vistaEspecialidad.Descripcion,
                        IdUsuarioRegistro = usuario.IdUsuario
                    };

                    objAdministracion.GuardarEspecialidad(especialidad);

                    vistaEspecialidad = new EspecialidadesView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Especialidad Guardada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_Especialidades", vistaEspecialidad);
        }

        public ActionResult ActualizarEspecialidad(EspecialidadesView vistaEspecialidad)
        {
            ViewBag.Title = "Gestión de especialidades";

            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (ModelState.IsValid)
                {
                    var especialidades = objAdministracion.ConsultarEspecialidades(true);

                    if (especialidades.Exists(e => e.Nombre == vistaEspecialidad.NombreEspecialidad && e.IdEspecialidad != vistaEspecialidad.IdEspecialidad))
                        throw new Exception("Ya existe una especilidad con ese nombre.");

                    Especialidad especialidad = new Especialidad()
                    {
                        IdEspecialidad = vistaEspecialidad.IdEspecialidad,
                        Nombre = vistaEspecialidad.NombreEspecialidad,
                        Descripcion = vistaEspecialidad.Descripcion,
                        Estado = vistaEspecialidad.EstadoOriginalEspecialidad ? true : vistaEspecialidad.Estado == 1,
                        IdUsuarioModificacion = usuario.IdUsuario
                    };

                    objAdministracion.ActualizarEspecialidad(especialidad);

                    vistaEspecialidad = new EspecialidadesView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Especialidad Actualizada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_Especialidades", vistaEspecialidad);
        }

        public ActionResult EliminarEspecialidad(EspecialidadesView vistaEspecialidad)
        {
            ViewBag.Title = "Gestión de especialidades";

            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (ModelState.IsValid)
                {
                    var medicosEspecialidad = objAdministracion.ConsultarMedicosEspecialidad(vistaEspecialidad.IdEspecialidad);

                    if (medicosEspecialidad != null && medicosEspecialidad.Count() > 0)
                        throw new Exception("Especialidad no puede ser eliminada por tener médicos asignados");

                    Especialidad especialidad = new Especialidad()
                    {
                        IdEspecialidad = vistaEspecialidad.IdEspecialidad,
                        IdUsuarioModificacion = usuario.IdUsuario
                    };

                    objAdministracion.EliminarEspecialidad(especialidad);

                    vistaEspecialidad = new EspecialidadesView();

                    ModelState.Clear();
                }

                ViewBag.Message = "Especialidad Eliminada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_Especialidades", vistaEspecialidad);
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

            AdministracionCore objAdministracion = new AdministracionCore();

            PacienteLineaView vistaPaciente = new PacienteLineaView();

            try
            {
                var tipoIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tipoIdentificaciones != null)
                    tipoIdentificaciones = tipoIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                vistaPaciente.ListaTiposIdentificaciones = tipoIdentificaciones;

                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (usuario.IdRolSeguridad == 2)    // Paciente
                {
                    vistaPaciente.EsPaciente = true;

                    if (usuario.IdPersona.GetValueOrDefault() > 0)
                    {
                        var persona = objAdministracion.ConsultarPersonaId(usuario.IdPersona.GetValueOrDefault());
                        var paciente = objAdministracion.ConsultarPaciente(persona.IdTipoIdentificacion, persona.Identificacion);

                        if (paciente != null && paciente.EstadoTipo)
                        {
                            vistaPaciente.IdPersona = paciente.IdPersona;
                            vistaPaciente.IdTipoIdentificacion = paciente.IdTipoIdentificacion;
                            vistaPaciente.Identificacion = paciente.Identificacion;
                            vistaPaciente.Nombres = paciente.Nombres;
                            vistaPaciente.Apellidos = paciente.Apellidos;
                            vistaPaciente.Email = paciente.Email;
                            vistaPaciente.IdTipoPersona = paciente.IdTipo;
                        }
                        else
                            throw new Exception("Paciente se encuentra inactivo. Acerquese al hospital.");
                    }
                    else
                        throw new Exception("Usuario no asignado. Acerquese al hospital.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_PacienteLinea", vistaPaciente);
        }

        public ActionResult GuardarPacienteLinea(PacienteLineaView vistaPaciente)
        {
            ViewBag.Title = "Gestión de registro en línea";

            AdministracionCore objAdministracion = new AdministracionCore();

            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                if (ModelState.IsValid)
                {
                    Persona persona = new Persona()
                    {
                        IdPersona = vistaPaciente.IdPersona,
                        IdTipoIdentificacion = vistaPaciente.IdTipoIdentificacion,
                        Identificacion = vistaPaciente.Identificacion,
                        Nombres = vistaPaciente.Nombres,
                        Apellidos = vistaPaciente.Apellidos,
                        Email = vistaPaciente.Email
                    };

                    objAdministracion.GuardarPersona(persona);

                    if (vistaPaciente.EsPaciente)
                    {
                        vistaPaciente = new PacienteLineaView();

                        ModelState.Clear();
                    }
                }

                ViewBag.Message = "Paciente Guardado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPaciente.ListaTiposIdentificaciones = tiposIdentificaciones;

            return View("_PacienteLinea", vistaPaciente);
        }

        public ActionResult ActualizarPacienteLinea(PacienteLineaView vistaPaciente)
        {
            ViewBag.Title = "Gestión de pacientes en Línea";

            AdministracionCore objAdministracion = new AdministracionCore();

            List<DetalleCatalogo> tiposIdentificaciones = new List<DetalleCatalogo>();

            try
            {
                tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                if (ModelState.IsValid)
                {
                    Paciente paciente = new Paciente()
                    {
                        IdPersona = vistaPaciente.IdPersona,
                        IdTipoIdentificacion = vistaPaciente.IdTipoIdentificacion,
                        Identificacion = vistaPaciente.Identificacion,
                        Nombres = vistaPaciente.Nombres,
                        Apellidos = vistaPaciente.Apellidos,
                        Email = vistaPaciente.Email
                    };

                    objAdministracion.ActualizarPaciente(paciente);

                    if (!vistaPaciente.EsPaciente)
                    {
                        vistaPaciente = new PacienteLineaView();

                        ModelState.Clear();
                    }
                }

                ViewBag.Message = "Paciente Actualizado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaPaciente.ListaTiposIdentificaciones = tiposIdentificaciones;

            return View("_PacienteLinea", vistaPaciente);
        }

        #endregion

        #region Especialidades

        [AuthorizeUser(idOperacion: 12)]
        public ActionResult Catalogos()
        {
            ViewBag.Title = "Gestión de catálogos";

            AdministracionCore objAdministracion = new AdministracionCore();

            DetallesCatalogoView vistaDetalles = new DetallesCatalogoView();

            var listaCatalogos = objAdministracion.ConsultarCatalogos();

            vistaDetalles.ListaCatalogos = listaCatalogos.FindAll(l => l.Administrable).OrderBy(c => c.Nombre).ToList();

            return View("_DetallesCatalogo", vistaDetalles);
        }

        public JsonResult CargarGridDetallesCatalogo([DataSourceRequest] DataSourceRequest request, int idCatalogo)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                List<DetalleCatalogo> detallesCatalogo = null;

                if (idCatalogo > 0)
                {
                    detallesCatalogo = objAdministracion.ConsultarDetallesCatalogo(idCatalogo, true);

                    if (detallesCatalogo != null)
                    {
                        detallesCatalogo = detallesCatalogo.OrderBy(e => e.IdDetalleCatalogo).ToList();
                    }
                }

                if (detallesCatalogo == null)
                    detallesCatalogo = new List<DetalleCatalogo>();

                return Json(detallesCatalogo.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarDetalleCatalogo(DetallesCatalogoView vistaDetalles)
        {
            ViewBag.Title = "Gestión de catálogos";

            AdministracionCore objAdministracion = new AdministracionCore();

            List<Catalogo> listaCatalogos = new List<Catalogo>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                listaCatalogos = objAdministracion.ConsultarCatalogos();
                listaCatalogos = listaCatalogos.FindAll(l => l.Administrable).OrderBy(c => c.Nombre).ToList();

                if (ModelState.IsValid)
                {
                    DetalleCatalogo detalleCatalogo = new DetalleCatalogo()
                    {
                        IdCatalogo = vistaDetalles.IdCatalogo,
                        Nombre = vistaDetalles.NombreDetalle,
                        Codigo = vistaDetalles.CodigoDetalle
                    };

                    objAdministracion.GuardarDetalleCatalogo(detalleCatalogo, usuario);

                    int idCatalogo = vistaDetalles.IdCatalogo;

                    vistaDetalles = new DetallesCatalogoView()
                    {
                        IdCatalogo = idCatalogo
                    };

                    ModelState.Clear();

                    ViewBag.Message = "Detalle Guardado Correctamente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaDetalles.ListaCatalogos = listaCatalogos;

            return View("_DetallesCatalogo", vistaDetalles);
        }

        public ActionResult ActualizarDetalleCatalogo(DetallesCatalogoView vistaDetalles)
        {
            ViewBag.Title = "Gestión de catálogos";

            AdministracionCore objAdministracion = new AdministracionCore();

            List<Catalogo> listaCatalogos = new List<Catalogo>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                listaCatalogos = objAdministracion.ConsultarCatalogos();
                listaCatalogos = listaCatalogos.FindAll(l => l.Administrable).OrderBy(c => c.Nombre).ToList();

                if (ModelState.IsValid)
                {
                    DetalleCatalogo detalleCatalogo = new DetalleCatalogo()
                    {
                        IdCatalogo = vistaDetalles.IdCatalogo,
                        IdDetalleCatalogo = vistaDetalles.IdDetalleCatalogo,
                        Nombre = vistaDetalles.NombreDetalle,
                        Codigo = vistaDetalles.CodigoDetalle,
                        Estado = vistaDetalles.EstadoOriginalDetalleCatalogo ? true : vistaDetalles.EstadoDetalle == 1
                    };

                    objAdministracion.ActualizarDetalleCatalogo(detalleCatalogo, usuario);

                    int idCatalogo = vistaDetalles.IdCatalogo;

                    vistaDetalles = new DetallesCatalogoView()
                    {
                        IdCatalogo = idCatalogo
                    };

                    ModelState.Clear();

                    ViewBag.Message = "Detalle Actualizado Correctamente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaDetalles.ListaCatalogos = listaCatalogos;

            return View("_DetallesCatalogo", vistaDetalles);
        }

        public ActionResult EliminarDetalleCatalogo(DetallesCatalogoView vistaDetalles)
        {
            ViewBag.Title = "Gestión de catálogos";

            AdministracionCore objAdministracion = new AdministracionCore();

            List<Catalogo> listaCatalogos = new List<Catalogo>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                listaCatalogos = objAdministracion.ConsultarCatalogos();
                listaCatalogos = listaCatalogos.FindAll(l => l.Administrable).OrderBy(c => c.Nombre).ToList();

                if (ModelState.IsValid)
                {
                    DetalleCatalogo detalleCatalogo = new DetalleCatalogo()
                    {
                        IdDetalleCatalogo = vistaDetalles.IdDetalleCatalogo
                    };

                    objAdministracion.EliminarDetalleCatalogo(detalleCatalogo, usuario);

                    int idCatalogo = vistaDetalles.IdCatalogo;

                    vistaDetalles = new DetallesCatalogoView()
                    {
                        IdCatalogo = idCatalogo
                    };

                    ModelState.Clear();

                    ViewBag.Message = "Detalle Eliminado Correctamente";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaDetalles.ListaCatalogos = listaCatalogos;

            return View("_DetallesCatalogo", vistaDetalles);
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