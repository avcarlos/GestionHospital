using GestionHospital.DataAccess;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }

            return citas;
        }

        public List<Horario> ConsultarHorariosDisponiblesCita(int idMedico, DateTime fecha)
        {
            AdministracionCore objAdministracion = new AdministracionCore();

            var horarios = objAdministracion.ConsultarHorarios(null, 102);
            var citasAgendadas = ConsultarCitasMedicas(null, null, idMedico, 15, null, fecha, null);

            horarios = horarios.FindAll(h => !citasAgendadas.Exists(c => c.IdHorario == h.IdHorario));

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
    }
}