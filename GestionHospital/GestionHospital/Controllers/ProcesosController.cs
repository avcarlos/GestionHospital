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
                vistaCitas.ListaMedicamentos = objAdministracion.ConsultarDetallesCatalogo(101);
                vistaCitas.ListaExamenes = objAdministracion.ConsultarDetallesCatalogo(102);

                vistaCitas.IdTemp = Guid.NewGuid().ToString();

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
                {
                    citasPendientes.ForEach(c => { if (c.Receta == null) c.Receta = new Receta(); });

                    citasPendientes = citasPendientes.OrderBy(c => c.Fecha).OrderBy(c => c.IdHorario).ToList();
                }

                return Json(citasPendientes.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarDetalleReceta([DataSourceRequest] DataSourceRequest request, int idReceta, string idTemp)
        {
            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                List<DetalleReceta> detalles;

                if (idReceta > 0)
                {
                    detalles = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idReceta.ToString()];

                    if (detalles == null)
                    {
                        detalles = objProcesos.ConsultarDetallesReceta(idReceta);

                        Session["DetallesReceta-" + idReceta.ToString()] = detalles;
                    }
                }
                else
                {
                    detalles = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idTemp];

                    if (detalles == null)
                    {
                        detalles = new List<DetalleReceta>();

                        Session["DetallesReceta-" + idTemp] = detalles;
                    }
                }

                return Json(detalles.OrderBy(t => t.IdDetalleReceta).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ActualizarDetalleReceta(DetalleReceta detalle, int idReceta, string idTemp)
        {
            try
            {
                List<DetalleReceta> detallesReceta;

                if (idReceta > 0)
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idReceta.ToString()];
                else
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idTemp];

                if (detallesReceta == null)
                    detallesReceta = new List<DetalleReceta>();

                var detalleAnterior = detallesReceta.FirstOrDefault(e => e.IdDetalleReceta == detalle.IdDetalleReceta);

                detallesReceta.Remove(detalleAnterior);
                detallesReceta.Add(detalle);

                if (idReceta > 0)
                    Session["DetallesReceta-" + idReceta.ToString()] = detallesReceta;
                else
                    Session["DetallesReceta-" + idTemp] = detallesReceta;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult GuardarDetalleReceta(DetalleReceta detalles, int idReceta, string idTemp)
        {
            try
            {
                List<DetalleReceta> detallesReceta;

                if (idReceta > 0)
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idReceta.ToString()];
                else
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idTemp];

                if (detallesReceta == null)
                    detallesReceta = new List<DetalleReceta>();

                if (detallesReceta.Count() > 0)
                    detalles.IdDetalleReceta = detallesReceta.Min(e => e.IdDetalleReceta) < 0 ? detallesReceta.Min(e => e.IdDetalleReceta) - 1 : -1;
                else
                    detalles.IdDetalleReceta = -1;

                detallesReceta.Add(detalles);

                if (idReceta > 0)
                    Session["DetallesReceta-" + idReceta.ToString()] = detallesReceta;
                else
                    Session["DetallesReceta-" + idTemp] = detallesReceta;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult EliminarDetalleReceta(DetalleReceta detalle, int idReceta, string idTemp)
        {
            try
            {
                List<DetalleReceta> detallesReceta;

                if (idReceta > 0)
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idReceta.ToString()];
                else
                    detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + idTemp];

                if (detallesReceta == null)
                    detallesReceta = new List<DetalleReceta>();

                var detalleEliminar = detallesReceta.FirstOrDefault(e => e.IdDetalleReceta == detalle.IdDetalleReceta);

                detallesReceta.Remove(detalleEliminar);

                if (idReceta > 0)
                    Session["DetallesReceta-" + idReceta.ToString()] = detallesReceta;
                else
                    Session["DetallesReceta-" + idTemp] = detallesReceta;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ConsultarExamenesCita([DataSourceRequest] DataSourceRequest request, int idCita)
        {
            ProcesosCore objProcesos = new ProcesosCore();

            try
            {
                var examenes = (List<ExamenMedico>)System.Web.HttpContext.Current.Session["ExamenesCita-" + idCita.ToString()];

                if (examenes == null)
                {
                    examenes = objProcesos.ConsultarExamenesCita(idCita);

                    Session["ExamenesCita-" + idCita.ToString()] = examenes;
                }

                return Json(examenes.OrderBy(t => t.IdExamenCita).ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult ActualizarExamenCita(ExamenMedico examen, int idCita)
        {
            try
            {
                var examenes = (List<ExamenMedico>)System.Web.HttpContext.Current.Session["ExamenesCita-" + idCita.ToString()];

                if (examenes == null)
                    examenes = new List<ExamenMedico>();

                var detalleAnterior = examenes.FirstOrDefault(e => e.IdExamenCita == examen.IdExamenCita);

                examenes.Remove(detalleAnterior);
                examenes.Add(examen);

                Session["ExamenesCita-" + idCita.ToString()] = examenes;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult GuardarExamenCita(ExamenMedico examen, int idCita)
        {
            try
            {
                var examenes = (List<ExamenMedico>)System.Web.HttpContext.Current.Session["ExamenesCita-" + idCita.ToString()];

                if (examenes == null)
                    examenes = new List<ExamenMedico>();

                if (examenes.Count() > 0)
                    examen.IdExamenCita = examenes.Min(e => e.IdExamenCita) < 0 ? examenes.Min(e => e.IdExamenCita) - 1 : -1;
                else
                    examen.IdExamenCita = -1;

                examenes.Add(examen);

                Session["ExamenesCita-" + idCita.ToString()] = examenes;

                return Json("OK");
            }
            catch (Exception ex)
            {
                return RetornarErrorJsonResult(ex.Message);
            }
        }

        public JsonResult EliminarExamenCita(ExamenMedico examen, int idCita)
        {
            try
            {
                var examenes = (List<ExamenMedico>)System.Web.HttpContext.Current.Session["ExamenesCita-" + idCita.ToString()];

                if (examenes == null)
                    examenes = new List<ExamenMedico>();

                var examenEliminar = examenes.FirstOrDefault(e => e.IdExamenCita == examen.IdExamenCita);

                examenes.Remove(examenEliminar);

                Session["ExamenesCita-" + idCita.ToString()] = examenes;

                return Json("OK");
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
            List<DetalleCatalogo> listaMedicamentos = new List<DetalleCatalogo>();
            List<DetalleCatalogo> listaExamenes = new List<DetalleCatalogo>();

            try
            {
                var usuario = (Usuario)System.Web.HttpContext.Current.Session["Usuario"];

                DateTime fechaConsulta = vistaCitas.FechaConsulta;
                listaEstados = objAdministracion.ConsultarDetallesCatalogo(4);
                listaMedicamentos = objAdministracion.ConsultarDetallesCatalogo(101);
                listaExamenes = objAdministracion.ConsultarDetallesCatalogo(102);

                if (ModelState.IsValid)
                {
                    string identificacion = vistaCitas.IdentificacionMedico;

                    List<DetalleReceta> detallesReceta;

                    if (vistaCitas.Receta.IdReceta > 0)
                        detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + vistaCitas.Receta.IdReceta.ToString()];
                    else
                        detallesReceta = (List<DetalleReceta>)System.Web.HttpContext.Current.Session["DetallesReceta-" + vistaCitas.IdTemp];

                    vistaCitas.Receta.IdCita = vistaCitas.IdCita;
                    vistaCitas.Receta.Detalles = detallesReceta;

                    var examenesCita = (List<ExamenMedico>)System.Web.HttpContext.Current.Session["ExamenesCita-" + vistaCitas.IdCita.ToString()];

                    CitaMedica cita = new CitaMedica()
                    {
                        IdCita = vistaCitas.IdCita,
                        Diagnostico = vistaCitas.Diagnostico,
                        Receta = vistaCitas.Receta,
                        IdEstado = vistaCitas.IdEstadoCita,
                        FechaProximoControl = vistaCitas.FechaProximoControl,
                        Examenes = examenesCita
                    };

                    objProcesos.GuardarResultadoCita(cita, usuario);

                    string idTemp = vistaCitas.IdTemp;

                    vistaCitas = new GestionCitasView
                    {
                        IdentificacionMedico = identificacion,
                        FechaConsulta = fechaConsulta,
                        IdTemp = idTemp
                    };

                    Session["DetallesReceta-" + idTemp] = null;
                    Session["DetallesReceta-" + cita.Receta.IdReceta.ToString()] = null;
                    Session["ExamenesCita-" + cita.IdCita.ToString()] = null;

                    ModelState.Clear();
                }

                ViewBag.Message = "Cita Guardada Correctamente";
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = ex.Message;
            }

            vistaCitas.FechaActual = fechaActual;
            vistaCitas.ListaEstados = listaEstados;
            vistaCitas.ListaMedicamentos = listaMedicamentos;
            vistaCitas.ListaExamenes = listaExamenes;

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