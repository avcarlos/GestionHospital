using GestionHospital.DataAccess;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GestionHospital.Logica
{
    public class ProcesosCore
    {
        private static DBManager GetConnection() => new DBManager();

        #region Comunes

        public List<CitaMedica> ConsultarCitasMedicas(int? idCita, int? idPaciente, int? idMedico, int? idEstado, int? idEspecialidad, DateTime? fecha, DateTime? fechaProxima)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[7]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_paciente", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_estado", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_especialidad", SqlDbType.Int, 4),
                objData.CreateParameter("@i_fecha", SqlDbType.DateTime, 8),
                objData.CreateParameter("@i_fecha_proximo_control", SqlDbType.DateTime, 8)
            };

            if (idCita != null)
                parameters[0].Value = idCita.GetValueOrDefault();
            if (idPaciente != null)
                parameters[1].Value = idPaciente.GetValueOrDefault();
            if (idMedico != null)
                parameters[2].Value = idMedico.GetValueOrDefault();
            if (idEstado != null)
                parameters[3].Value = idEstado.GetValueOrDefault();
            if (idEspecialidad != null)
                parameters[4].Value = idEspecialidad.GetValueOrDefault();
            if (fecha != null)
                parameters[5].Value = fecha.GetValueOrDefault();
            if (fechaProxima != null)
                parameters[5].Value = fechaProxima.GetValueOrDefault();

            var citas = objData.ConsultarDatos<CitaMedica>("ConsultarCitas", parameters);

            return citas;
        }

        public CitaMedica ConsultarDatosCompletosCitas(CitaMedica cita)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            var medico = objAdministracion.ConsultarPersonaId(cita.IdMedico);

            cita.NombreEspecialidad = objAdministracion.ConsultarEspecialidad(cita.IdEspecialidad).Nombre;
            cita.NombreMedico = medico.Nombres + " " + medico.Apellidos;
            cita.NombreHorario = objAdministracion.ConsultarHorario(cita.IdHorario).Nombre;

            if (cita.IdPaciente > 0)
            {
                var paciente = objAdministracion.ConsultarPersonaId(cita.IdPaciente);

                cita.NombrePaciente = paciente.Nombres + " " + paciente.Apellidos;

                if (paciente.FechaNacimiento != null)
                    cita.EdadPaciente = DateTime.Today.AddTicks(-paciente.FechaNacimiento.GetValueOrDefault().Ticks).Year - 1;
            }

            return cita;
        }

        #endregion

        #region Agendamiento Citas

        public List<CitaMedica> ConsultarCitasPendientesPaciente(int idPaciente)
        {
            var citas = ConsultarCitasMedicas(null, idPaciente, null, 15, null, null, null);

            foreach (var cita in citas)
            {
                var datosCita = ConsultarDatosCompletosCitas(cita);

                cita.NombreEspecialidad = datosCita.NombreEspecialidad;
                cita.NombreMedico = datosCita.NombreMedico;
                cita.NombreHorario = datosCita.NombreHorario;
                cita.NombrePaciente = datosCita.NombrePaciente;
                cita.EdadPaciente = datosCita.EdadPaciente;
            }

            return citas;
        }

        public List<Horario> ConsultarHorariosDisponiblesCita(int idPaciente, int idMedico, DateTime fecha, int idCita)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            var horarios = objAdministracion.ConsultarHorarios(null, 102);
            var citasAgendadasMedico = ConsultarCitasMedicas(null, null, idMedico, 15, null, fecha, null);

            horarios = horarios.FindAll(h => !citasAgendadasMedico.Exists(c => c.IdHorario == h.IdHorario && c.IdCita != idCita));

            var citasAgendadasPaciente = ConsultarCitasMedicas(null, idPaciente, null, 15, null, fecha, null);

            horarios = horarios.FindAll(h => !citasAgendadasPaciente.Exists(c => c.IdHorario == h.IdHorario && c.IdCita != idCita));

            return horarios;
        }

        public void GuardarCita(CitaMedica cita)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[7]
            {
                objData.CreateParameter("@i_id_paciente", SqlDbType.Int, 4, cita.IdPaciente),
                objData.CreateParameter("@i_id_especialidad", SqlDbType.Int, 4, cita.IdEspecialidad),
                objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4, cita.IdMedico),
                objData.CreateParameter("@i_fecha", SqlDbType.DateTime, 8, cita.Fecha),
                objData.CreateParameter("@i_id_horario", SqlDbType.Int, 4, cita.IdHorario),
                objData.CreateParameter("@i_motivo", SqlDbType.VarChar, 300, cita.Motivo),
                objData.CreateParameter("@i_id_estado", SqlDbType.Int, 4, cita.IdEstado)
            };

            objData.Insert("GuardarCita", CommandType.StoredProcedure, parameters);
        }

        public void ActualizarCita(CitaMedica cita)
        {
            if (cita != null)
            {
                var objData = GetConnection();

                IDbDataParameter[] parameters = new IDbDataParameter[7]
                {
                    objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, cita.IdCita),
                    objData.CreateParameter("@i_id_especialidad", SqlDbType.Int, 4, cita.IdEspecialidad),
                    objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4, cita.IdMedico),
                    objData.CreateParameter("@i_fecha", SqlDbType.DateTime, 8, cita.Fecha),
                    objData.CreateParameter("@i_id_horario", SqlDbType.Int, 4, cita.IdHorario),
                    objData.CreateParameter("@i_motivo", SqlDbType.VarChar, 300, cita.Motivo),
                    objData.CreateParameter("@i_id_estado", SqlDbType.Int, 4, cita.IdEstado)
                };

                objData.Update("ActualizarCita", CommandType.StoredProcedure, parameters);
            }
        }

        public void EliminarCita(CitaMedica cita)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, cita.IdCita),
            };

            objData.Delete("EliminarCita", CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region Gestión Citas

        public List<CitaMedica> ConsultarCitasPendientesMedico(int idMedico, DateTime fecha)
        {
            var citas = ConsultarCitasMedicas(null, null, idMedico, 15, null, fecha, null);

            foreach (var cita in citas)
            {
                var datosCita = ConsultarDatosCompletosCitas(cita);

                cita.NombreEspecialidad = datosCita.NombreEspecialidad;
                cita.NombreMedico = datosCita.NombreMedico;
                cita.NombreHorario = datosCita.NombreHorario;
                cita.NombrePaciente = datosCita.NombrePaciente;
                cita.EdadPaciente = datosCita.EdadPaciente;
                cita.Receta = ConsultarReceta(cita.IdCita, null);
            }

            return citas;
        }

        public void GuardarDatosAdicionalesCita(CitaMedica cita, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[5]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, cita.IdCita),
                objData.CreateParameter("@i_diagnostico", SqlDbType.VarChar, 300, cita.Diagnostico),
                objData.CreateParameter("@i_id_estado", SqlDbType.Int, 4, cita.IdEstado),
                objData.CreateParameter("@i_fecha_proximo_control", SqlDbType.DateTime, 8),
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario)
            };

            if (cita.FechaProximoControl != null)
                parameters[3].Value = cita.FechaProximoControl.GetValueOrDefault();

            objData.Insert("GuardarDatosAdicionalesCita", CommandType.StoredProcedure, parameters);
        }

        public void GuardarResultadoCita(CitaMedica cita, Usuario usuario)
        {
            List<DetalleReceta> detallesRecetaGuardar = new List<DetalleReceta>();
            List<DetalleReceta> detallesRecetaEliminar = new List<DetalleReceta>();
            List<ExamenMedico> examenesGuardar = new List<ExamenMedico>();
            List<ExamenMedico> examenesEliminar = new List<ExamenMedico>();

            if (cita.Receta.IdReceta == 0)
            {
                var recetaAnterior = ConsultarReceta(cita.IdCita, null);

                if (recetaAnterior != null)
                    cita.Receta.IdReceta = recetaAnterior.IdReceta;
            }

            if (cita.Receta.IdReceta > 0)
            {
                var detallesRecetaAnteriores = ConsultarDetallesReceta(cita.Receta.IdReceta);

                if (cita.Receta.Detalles.Exists(d => d.IdDetalleReceta == 0))
                    detallesRecetaGuardar = cita.Receta.Detalles.FindAll(d => d.IdDetalleReceta == 0);

                if (detallesRecetaAnteriores.Exists(a => !cita.Receta.Detalles.Exists(d => a.IdDetalleReceta == d.IdDetalleReceta)))
                    detallesRecetaEliminar = detallesRecetaAnteriores.FindAll(a => !cita.Receta.Detalles.Exists(d => a.IdDetalleReceta == d.IdDetalleReceta));
            }
            else
                detallesRecetaGuardar = cita.Receta.Detalles;

            var examenesAnteriores = ConsultarExamenesCita(cita.IdCita);

            if (examenesAnteriores != null && examenesAnteriores.Count() > 0)
            {
                if (cita.Examenes.Exists(d => d.IdExamenCita == 0))
                    examenesGuardar = cita.Examenes.FindAll(d => d.IdExamenCita == 0);

                if (examenesAnteriores.Exists(a => !cita.Examenes.Exists(d => a.IdExamenCita == d.IdExamenCita)))
                    examenesEliminar = examenesAnteriores.FindAll(a => !cita.Examenes.Exists(d => a.IdExamenCita == d.IdExamenCita));
            }
            else
                examenesGuardar = cita.Examenes;

            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                GuardarDatosAdicionalesCita(cita, usuario);

                int idReceta = GuardarReceta(cita.Receta, usuario);

                if (detallesRecetaGuardar != null)
                    foreach (var item in detallesRecetaGuardar)
                    {
                        item.IdReceta = idReceta;

                        GuardarDetalleReceta(item);
                    }

                if (detallesRecetaEliminar != null)
                    foreach (var item in detallesRecetaEliminar)
                    {
                        EliminarDetalleReceta(item);
                    }

                if(examenesGuardar != null)
                    foreach (var item in examenesGuardar)
                    {
                        GuardarExamenCita(item);
                    }

                if (examenesEliminar != null)
                    foreach (var item in examenesEliminar)
                    {
                        EliminarExamenCita(item);
                    }

                tran.Complete();
            }
        }

        #endregion

        #region Receta

        public Receta ConsultarReceta(int? idCita, int? idReceta)
        {
            var objData = GetConnection();

            Receta receta = null;

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_receta", SqlDbType.Int, 4)
            };

            if (idCita != null)
                parameters[0].Value = idCita.GetValueOrDefault();
            if (idReceta != null)
                parameters[1].Value = idReceta.GetValueOrDefault();

            var recetas = objData.ConsultarDatos<Receta>("ConsultarReceta", parameters);

            if (recetas != null && recetas.Count() > 0)
                receta = recetas.FirstOrDefault();

            return receta;
        }

        public int GuardarReceta(Receta receta, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[5]
            {
                objData.CreateParameter("@i_id_receta", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, receta.IdCita),
                objData.CreateParameter("@i_observaciones", SqlDbType.VarChar, 300, receta.Observaciones),
                objData.CreateParameter("@i_usuario", SqlDbType.Int, 4, usuario.IdUsuario),
                objData.CreateParameter("@o_id_receta", SqlDbType.Int, 4, ParameterDirection.Output)
            };

            if (receta.IdReceta > 0)
                parameters[0].Value = receta.IdReceta;

            var idReceta = objData.Insert("GuardarReceta", CommandType.StoredProcedure, parameters);

            return idReceta;
        }

        public List<DetalleReceta> ConsultarDetallesReceta(int idReceta)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_receta", SqlDbType.Int, 4, idReceta)
            };

            var detalles = objData.ConsultarDatos<DetalleReceta>("ConsultarDetallesReceta", parameters);

            return detalles;
        }

        public void GuardarDetalleReceta(DetalleReceta detalle)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_receta", SqlDbType.Int, 4, detalle.IdReceta),
                objData.CreateParameter("@i_id_medicamento", SqlDbType.Int, 4, detalle.IdMedicamento),
                objData.CreateParameter("@i_indicaciones", SqlDbType.VarChar, 150)
            };

            if (!string.IsNullOrEmpty(detalle.Indicaciones))
                parameters[2].Value = detalle.Indicaciones;

            objData.Insert("GuardarDetalleReceta", CommandType.StoredProcedure, parameters);
        }

        public void EliminarDetalleReceta(DetalleReceta detalle)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_detalle_receta", SqlDbType.Int, 4, detalle.IdDetalleReceta),
            };

            objData.Delete("EliminarDetalleReceta", CommandType.StoredProcedure, parameters);
        }

        public List<Receta> ConsultarRecetasPaciente(int? idPaciente, int? idReceta)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_paciente", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_receta", SqlDbType.Int, 4)
            };

            if (idPaciente != null)
                parameters[0].Value = idPaciente.GetValueOrDefault();
            if (idReceta != null)
                parameters[1].Value = idReceta.GetValueOrDefault();

            var recetas = objData.ConsultarDatos<Receta>("ConsultarRecetasPacientes", parameters);

            return recetas;
        }

        #endregion

        #region Exámenes

        public List<ExamenMedico> ConsultarExamenesCita(int idCita)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, idCita)
            };

            var examenes = objData.ConsultarDatos<ExamenMedico>("ConsultarExamenesCita", parameters);

            return examenes;
        }

        public void GuardarExamenCita(ExamenMedico examen)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_cita", SqlDbType.Int, 4, examen.IdCita),
                objData.CreateParameter("@i_id_examen", SqlDbType.Int, 4, examen.IdExamen),
                objData.CreateParameter("@i_indicaciones", SqlDbType.VarChar, 150)
            };

            if (!string.IsNullOrEmpty(examen.Indicaciones))
                parameters[2].Value = examen.Indicaciones;

            objData.Insert("GuardarExamenCita", CommandType.StoredProcedure, parameters);
        }

        public void EliminarExamenCita(ExamenMedico examen)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_examen_cita", SqlDbType.Int, 4, examen.IdExamenCita),
            };

            objData.Delete("EliminarExamenCita", CommandType.StoredProcedure, parameters);
        }

        #endregion
    }
}