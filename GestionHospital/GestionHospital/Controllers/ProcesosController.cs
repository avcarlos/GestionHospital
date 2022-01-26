using GestionHospital.Filters;
using GestionHospital.Logica;
using GestionHospital.Model.Shared;
using GestionHospital.Models.Procesos;
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
    public class ProcesosController : Controller
    {
        #region Agendamiento Cita

        [AuthorizeUser(idOperacion: 101)]
        public ActionResult AgendamientoCita()
        {
            ViewBag.Title = "Agendamiento de citas";

            AdministracionCore objAdministracion = new AdministracionCore();

            AgendamientoCitasView vistaAgendamiento = new AgendamientoCitasView();

            try
            {
                vistaAgendamiento.FechaActual = DateTime.Now;

                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (usuario.IdRolSeguridad == 2)    // Paciente
                {
                    vistaAgendamiento.EsPaciente = true;

                    if (usuario.IdPersona.GetValueOrDefault() > 0)
                    {
                        var persona = objAdministracion.ConsultarPersonaId(usuario.IdPersona.GetValueOrDefault());
                        var paciente = objAdministracion.ConsultarPaciente(persona.IdTipoIdentificacion, persona.Identificacion);

                        if (paciente != null && paciente.EstadoTipo)
                        {
                            vistaAgendamiento.IdPersona = paciente.IdPersona;
                            vistaAgendamiento.Identificacion = paciente.Identificacion;
                            vistaAgendamiento.NombrePaciente = paciente.Nombres + " " + paciente.Apellidos;
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

            return View("_AgendamientoCitas", vistaAgendamiento);
        }

        public JsonResult ConsultarPacienteCita(string identificacion)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                Paciente paciente = null;

                var tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                if (tiposIdentificaciones != null)
                    tiposIdentificaciones = tiposIdentificaciones.FindAll(t => t.Parametro1.GetValueOrDefault() == 1);

                foreach (var item in tiposIdentificaciones)
                {
                    paciente = objAdministracion.ConsultarPaciente(item.IdDetalleCatalogo, identificacion);

                    if (paciente != null)
                        break;
                }

                if (paciente != null)
                {
                    var data = new
                    {
                        idPersona = paciente.IdPersona,
                        nombrePaciente = paciente.Nombres + " " + paciente.Apellidos
                    };

                    return Json(data);
                }

                return Json("");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult CargarCitasPendientesPaciente([DataSourceRequest]
                                                    DataSourceRequest request, int idPaciente)
        {
            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                var citasPendientes = objProcesos.ConsultarCitasPendientesPaciente(idPaciente);

                if (citasPendientes == null)
                    citasPendientes = new List<CitaMedica>();
                else
                    citasPendientes = citasPendientes.OrderBy(c => c.Fecha).OrderBy(c => c.IdHorario).ToList();

                return Json(citasPendientes.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarEspecilidadesCita()
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var especialidades = objAdministracion.ConsultarEspecialidades();

                if (especialidades == null)
                    especialidades = new List<Especialidad>();
                else
                    especialidades = especialidades.OrderBy(e => e.IdEspecialidad).ToList();

                return Json(especialidades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarMedicosCita(int idEspecialidad)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                var medicos = objAdministracion.ConsultarMedicosEspecialidad(idEspecialidad);

                if (medicos == null)
                    medicos = new List<Medico>();
                else
                    medicos = medicos.OrderBy(m => m.NombreMedico).ToList();

                return Json(medicos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarHorariosCita(int idPaciente, int idMedico, string fechaCita, int idCita)
        {
            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                List<Horario> horarios = null;

                if (!string.IsNullOrEmpty(fechaCita))
                {
                    var fecha = Convert.ToDateTime(fechaCita);

                    horarios = objProcesos.ConsultarHorariosDisponiblesCita(idPaciente, idMedico, fecha, idCita);
                }

                if (horarios == null)
                    horarios = new List<Horario>();
                else
                    horarios = horarios.OrderBy(h => h.HoraInicio).ToList();

                return Json(horarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult AgendarCita(AgendamientoCitasView vistaAgendamiento)
        {
            ViewBag.Title = "Agendamiento de citas";

            ProcesosCore objProcesos = new ProcesosCore();

            DateTime fechaActual = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    string identificacion = vistaAgendamiento.Identificacion;

                    CitaMedica cita = new CitaMedica()
                    {
                        IdPaciente = vistaAgendamiento.IdPersona,
                        IdEspecialidad = vistaAgendamiento.IdEspecialidad,
                        IdMedico = vistaAgendamiento.IdMedico,
                        Fecha = vistaAgendamiento.FechaCita.GetValueOrDefault(),
                        IdHorario = vistaAgendamiento.IdHoraCita,
                        Motivo = vistaAgendamiento.Motivo,
                        IdEstado = 15
                    };

                    objProcesos.GuardarCita(cita);

                    vistaAgendamiento = new AgendamientoCitasView
                    {
                        Identificacion = identificacion
                    };

                    ModelState.Clear();
                }

                ViewBag.Message = "Cita Agendada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaAgendamiento.FechaActual = fechaActual;

            return View("_AgendamientoCitas", vistaAgendamiento);
        }

        public ActionResult ReagendarCita(AgendamientoCitasView vistaAgendamiento)
        {
            ViewBag.Title = "Agendamiento de citas";

            ProcesosCore objProcesos = new ProcesosCore();

            DateTime fechaActual = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    string identificacion = vistaAgendamiento.Identificacion;

                    CitaMedica cita = new CitaMedica()
                    {
                        IdCita = vistaAgendamiento.IdCita,
                        IdPaciente = vistaAgendamiento.IdPersona,
                        IdEspecialidad = vistaAgendamiento.IdEspecialidad,
                        IdMedico = vistaAgendamiento.IdMedico,
                        Fecha = vistaAgendamiento.FechaCita.GetValueOrDefault(),
                        IdHorario = vistaAgendamiento.IdHoraCita,
                        Motivo = vistaAgendamiento.Motivo,
                        IdEstado = 15
                    };

                    objProcesos.ActualizarCita(cita);

                    vistaAgendamiento = new AgendamientoCitasView
                    {
                        Identificacion = identificacion
                    };

                    ModelState.Clear();
                }

                ViewBag.Message = "Cita Reagendada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaAgendamiento.FechaActual = fechaActual;

            return View("_AgendamientoCitas", vistaAgendamiento);
        }

        public ActionResult CancelarCita(AgendamientoCitasView vistaAgendamiento)
        {
            ViewBag.Title = "Agendamiento de citas";

            ProcesosCore objProcesos = new ProcesosCore();

            DateTime fechaActual = DateTime.Now;

            try
            {
                if (ModelState.IsValid)
                {
                    string identificacion = vistaAgendamiento.Identificacion;

                    CitaMedica cita = new CitaMedica()
                    {
                        IdCita = vistaAgendamiento.IdCita
                    };

                    objProcesos.EliminarCita(cita);

                    vistaAgendamiento = new AgendamientoCitasView
                    {
                        Identificacion = identificacion
                    };

                    ModelState.Clear();
                }

                ViewBag.Message = "Médico Eliminado Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaAgendamiento.FechaActual = fechaActual;

            return View("_AgendamientoCitas", vistaAgendamiento);
        }

        #endregion

        #region Gestión Cita

        [AuthorizeUser(idOperacion: 111)]
        public ActionResult AgendaCitas()
        {
            ViewBag.Title = "Gestión de citas";

            AdministracionCore objAdministracion = new AdministracionCore();

            GestionCitasView vistaCitas = new GestionCitasView();

            try
            {
                vistaCitas.FechaActual = DateTime.Now;
                vistaCitas.FechaConsulta = vistaCitas.FechaActual;
                vistaCitas.ListaEstados = objAdministracion.ConsultarDetallesCatalogo(4);

                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                if (usuario.IdRolSeguridad == 5)    // Medico
                {
                    vistaCitas.EsMedico = true;

                    if (usuario.IdPersona.GetValueOrDefault() > 0)
                    {
                        var persona = objAdministracion.ConsultarPersonaId(usuario.IdPersona.GetValueOrDefault());
                        var medico = objAdministracion.ConsultarMedico(persona.IdTipoIdentificacion, persona.Identificacion);

                        if (medico != null && medico.EstadoTipo)
                        {
                            vistaCitas.IdMedico = medico.IdPersona;
                            vistaCitas.IdentificacionMedico = medico.Identificacion;
                            vistaCitas.NombreMedico = medico.Nombres + " " + medico.Apellidos;
                        }
                        else
                            throw new Exception("Médico se encuentra inactivo.");
                    }
                    else
                        throw new Exception("Usuario no asignado.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            return View("_GestionCitas", vistaCitas);
        }

        public JsonResult ConsultarMedicoCitas(string identificacion)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            try
            {
                Medico medico = null;

                var tiposIdentificaciones = objAdministracion.ConsultarDetallesCatalogo(3);

                foreach (var item in tiposIdentificaciones)
                {
                    medico = objAdministracion.ConsultarMedico(item.IdDetalleCatalogo, identificacion);

                    if (medico != null)
                        break;
                }

                if (medico != null)
                {
                    var data = new
                    {
                        idMedico = medico.IdPersona,
                        nombreMedico = medico.Nombres + " " + medico.Apellidos
                    };

                    return Json(data);
                }

                return Json("");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult CargarCitasPendientesMedico([DataSourceRequest]
                                                    DataSourceRequest request, int idMedico, string fechaCitas)
        {
            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                var fecha = Convert.ToDateTime(fechaCitas);

                var citasPendientes = objProcesos.ConsultarCitasPendientesMedico(idMedico, fecha);

                if (citasPendientes == null)
                    citasPendientes = new List<CitaMedica>();
                else
                    citasPendientes = citasPendientes.OrderBy(c => c.Fecha).OrderBy(c => c.IdHorario).ToList();

                return Json(citasPendientes.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public ActionResult GuardarDatosCita(GestionCitasView vistaCitas)
        {
            ViewBag.Title = "Gestión de citas";
            
            AdministracionCore objAdministracion = new AdministracionCore();
            ProcesosCore objProcesos = new ProcesosCore();

            DateTime fechaActual = DateTime.Now;

            List<DetalleCatalogo> listaEstados = new List<DetalleCatalogo>();

            try
            {
                DateTime fechaConsulta = vistaCitas.FechaConsulta;
                listaEstados = objAdministracion.ConsultarDetallesCatalogo(4);

                if (ModelState.IsValid)
                {
                    string identificacion = vistaCitas.IdentificacionMedico;

                    CitaMedica cita = new CitaMedica()
                    {
                        IdCita = vistaCitas.IdCita,
                        Diagnostico = vistaCitas.Diagnostico,
                        Examenes = vistaCitas.Examenes,
                        Receta = vistaCitas.Receta,
                        IdEstado = vistaCitas.IdEstadoCita,
                        FechaProximoControl = vistaCitas.FechaProximoControl
                    };

                    objProcesos.GuardarDatosAdicionalesCita(cita);

                    vistaCitas = new GestionCitasView
                    {
                        IdentificacionMedico = identificacion,
                        FechaConsulta = fechaConsulta
                    };

                    ModelState.Clear();
                }

                ViewBag.Message = "Cita Guardad Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaCitas.FechaActual = fechaActual;
            vistaCitas.ListaEstados = listaEstados;

            return View("_GestionCitas", vistaCitas);
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