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
    public class AdministracionCore
    {
        private static DBManager GetConnection() => new DBManager();

        #region Comun

        public List<DetalleCatalogo> ConsultarDetallesCatalogo(int idCatalogo)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_catalogo", SqlDbType.Int, 4, idCatalogo)
            };

            var detalles = objData.ConsultarDatos<DetalleCatalogo>("ConsultarDetalleCatalogo", parameters);

            return detalles;
        }

        public DetalleCatalogo ConsultarDetalleCatalogo(int idDetalleCatalogo, int idCatalogo = 0)
        {
            var objData = GetConnection();

            DetalleCatalogo detalle = null;

            IDbDataParameter[] parameters;

            parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_catalogo", SqlDbType.Int, 4),
                objData.CreateParameter("@i_id_detalle_catalogo", SqlDbType.Int, 4, idDetalleCatalogo)
            };

            if (idCatalogo != 0)
                parameters[0].Value = idCatalogo;

            var detalles = objData.ConsultarDatos<DetalleCatalogo>("ConsultarDe talleCatalogo", parameters);

            if (detalles != null && detalles.Count() > 0)
                detalle = detalles.FirstOrDefault();

            return detalle;
        }

        #endregion

        #region Persona

        public Persona ConsultarPersona(int idTipoIdentificacion, string identificacion, int? idTipoPersona = null)
        {
            var objData = GetConnection();

            Persona persona = null;

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_tipo_identificacion", SqlDbType.Int, 4, idTipoIdentificacion),
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, identificacion),
                objData.CreateParameter("@i_id_tipo_persona", SqlDbType.Int, 4)
            };

            if (idTipoPersona != null)
                parameters[2].Value = idTipoPersona.GetValueOrDefault();

            var personas = objData.ConsultarDatos<Persona>("ConsultarPersona", parameters);

            if (personas != null && personas.Count() > 0)
                persona = personas.FirstOrDefault();

            return persona;
        }

        public Persona ConsultarPersonaId(int idPersona)
        {
            var objData = GetConnection();

            Persona persona = null;

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, idPersona)
            };

            var personas = objData.ConsultarDatos<Persona>("ConsultarPersonaId", parameters);

            if (personas != null && personas.Count() > 0)
                persona = personas.FirstOrDefault();

            return persona;
        }

        public int GuardarPersona(Persona persona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[12]
            {
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, persona.Identificacion),
                objData.CreateParameter("@i_id_tipo_identificacion", SqlDbType.Int, 4, persona.IdTipoIdentificacion),
                objData.CreateParameter("@i_nombres", SqlDbType.VarChar, 50, persona.Nombres),
                objData.CreateParameter("@i_apellidos", SqlDbType.VarChar, 50, persona.Apellidos),
                objData.CreateParameter("@i_fecha_nacimiento", SqlDbType.DateTime, 8),
                objData.CreateParameter("@i_telefono", SqlDbType.VarChar, 12),
                objData.CreateParameter("@i_celular", SqlDbType.VarChar, 12),
                objData.CreateParameter("@i_email", SqlDbType.VarChar, 30),
                objData.CreateParameter("@i_id_genero", SqlDbType.Int, 4),
                objData.CreateParameter("@i_direccion", SqlDbType.VarChar, 300),
                objData.CreateParameter("@i_id_ciudad", SqlDbType.Int, 4),
                objData.CreateParameter("@o_id_persona", SqlDbType.Int, 4, ParameterDirection.Output)
            };

            if (persona.FechaNacimiento != null)
                parameters[4].Value = persona.FechaNacimiento.GetValueOrDefault();
            if (!string.IsNullOrEmpty(persona.Telefono))
                parameters[5].Value = persona.Telefono;
            if (!string.IsNullOrEmpty(persona.Celular))
                parameters[6].Value = persona.Celular;
            if (!string.IsNullOrEmpty(persona.Email))
                parameters[7].Value = persona.Email;
            if (persona.IdGenero != 0)
                parameters[8].Value = persona.IdGenero;
            if (!string.IsNullOrEmpty(persona.Direccion))
                parameters[9].Value = persona.Direccion;
            if (persona.IdCiudad != 0)
                parameters[10].Value = persona.IdCiudad;

            var idPersona = objData.Insert("GuardarPersona", CommandType.StoredProcedure, parameters);

            return idPersona;
        }

        public void ActualizarPersona(Persona persona)
        {
            if (persona != null)
            {
                var objData = GetConnection();

                IDbDataParameter[] parameters = new IDbDataParameter[8]
                {
                    objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, persona.IdPersona),
                    objData.CreateParameter("@i_fecha_nacimiento", SqlDbType.DateTime, 8),
                    objData.CreateParameter("@i_telefono", SqlDbType.VarChar, 12),
                    objData.CreateParameter("@i_celular", SqlDbType.VarChar, 12),
                    objData.CreateParameter("@i_email", SqlDbType.VarChar, 30),
                    objData.CreateParameter("@i_id_genero", SqlDbType.Int, 4),
                    objData.CreateParameter("@i_direccion", SqlDbType.VarChar, 300),
                    objData.CreateParameter("@i_id_ciudad", SqlDbType.Int, 4)
                };

                if (persona.FechaNacimiento != null)
                    parameters[1].Value = persona.FechaNacimiento.GetValueOrDefault();
                if (!string.IsNullOrEmpty(persona.Telefono))
                    parameters[2].Value = persona.Telefono;
                if (!string.IsNullOrEmpty(persona.Celular))
                    parameters[3].Value = persona.Celular;
                if (!string.IsNullOrEmpty(persona.Email))
                    parameters[4].Value = persona.Email;
                if (persona.IdGenero != 0)
                    parameters[5].Value = persona.IdGenero;
                if (!string.IsNullOrEmpty(persona.Direccion))
                    parameters[6].Value = persona.Direccion;
                if (persona.IdCiudad != 0)
                    parameters[7].Value = persona.IdCiudad;

                objData.Update("ActualizarPersona", CommandType.StoredProcedure, parameters);
            }
        }

        public void EliminarPersona(int idPersona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, idPersona),
            };

            objData.Delete("EliminarPersona", CommandType.StoredProcedure, parameters);
        }

        public List<TipoPersona> ConsultarTiposPersona(int idPersona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, idPersona)
            };

            var tipos = objData.ConsultarDatos<TipoPersona>("ConsultarTipoPersona", parameters);

            return tipos;
        }

        public void GuardarTipoPersona(TipoPersona tipoPersona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, tipoPersona.IdPersona),
                objData.CreateParameter("@i_id_tipo", SqlDbType.Int, 4, tipoPersona.IdTipo)
            };

            objData.Insert("GuardarTipoPersona", CommandType.StoredProcedure, parameters);
        }

        public void EliminarTipoPersona(TipoPersona tipoPersona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, tipoPersona.IdPersona),
                objData.CreateParameter("@i_id_tipo", SqlDbType.Int, 4, tipoPersona.IdTipo)
            };

            objData.Delete("EliminarTipoPersona", CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region Medico

        public Medico ConsultarMedico(int idTipoIdentificacion, string identificacion)
        {
            Medico medico = ConsultarDatosMedico(idTipoIdentificacion, identificacion);

            if (medico != null)
            {
                medico.Especialidades = ConsultarEspecialidadesMedico(medico.IdPersona);
            }

            return medico;
        }

        public Medico ConsultarDatosMedico(int idTipoIdentificacion, string identificacion)
        {
            var objData = GetConnection();

            Medico medico = null;

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_tipo_identificacion", SqlDbType.Int, 4, idTipoIdentificacion),
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, identificacion),
                objData.CreateParameter("@i_id_tipo_persona", SqlDbType.Int, 4, 5)
            };

            var medicos = objData.ConsultarDatos<Medico>("ConsultarPersona", parameters);

            if (medicos != null && medicos.Count() > 0)
                medico = medicos.FirstOrDefault();

            return medico;
        }

        public void GuardarMedico(Medico medico)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                if (medico.IdPersona == 0)
                    medico.IdPersona = GuardarPersona(medico);
                else
                    ActualizarPersona(medico);

                GuardarTipoPersona(new TipoPersona() { IdPersona = medico.IdPersona, IdTipo = 5 });

                if (medico.Especialidades != null && medico.Especialidades.Count() > 0)
                {
                    foreach (var item in medico.Especialidades)
                    {
                        item.IdMedico = medico.IdPersona;

                        GuardarEspecialidadMedico(item);
                    }
                }

                tran.Complete();
            }
        }

        public void ActualizarMedico(Medico medico)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                ActualizarPersona(medico);

                var tipos = ConsultarTiposPersona(medico.IdPersona);
                var tipoMedico = tipos.FirstOrDefault(t => t.IdTipo == 5);

                if (!tipoMedico.Estado && medico.EstadoTipo)
                    GuardarTipoPersona(new TipoPersona() { IdPersona = medico.IdPersona, IdTipo = 5 });

                if (medico.Especialidades != null && medico.Especialidades.Count() > 0)
                {
                    var especialidadesIniciales = ConsultarEspecialidadesMedico(medico.IdPersona);

                    var especialidadesGuardar = medico.Especialidades.FindAll(e => !especialidadesIniciales.Exists(i => i.IdEspecialidad == e.IdEspecialidad));
                    var especialidadesEliminar = especialidadesIniciales.FindAll(i => !medico.Especialidades.Exists(e => e.IdEspecialidad == i.IdEspecialidad));

                    foreach (var item in especialidadesGuardar)
                    {
                        item.IdMedico = medico.IdPersona;

                        GuardarEspecialidadMedico(item);
                    }

                    foreach (var item in especialidadesEliminar)
                    {
                        item.IdMedico = medico.IdPersona;

                        EliminarEspecialidadMedico(item);
                    }
                }

                tran.Complete();
            }
        }

        public void EliminarMedico(Medico medico)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                EliminarTipoPersona(new TipoPersona() { IdPersona = medico.IdPersona, IdTipo = 5 });

                tran.Complete();
            }
        }

        public List<Especialidad> ConsultarEspecialidadesMedico(int idMedico)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4, idMedico)
            };

            var especialidades = objData.ConsultarDatos<Especialidad>("ConsultarEspecialidadesMedico", parameters);

            return especialidades;
        }

        public void GuardarEspecialidadMedico(Especialidad especialidad)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4, especialidad.IdMedico),
                objData.CreateParameter("@i_id_especialidad", SqlDbType.Int, 4, especialidad.IdEspecialidad)
            };

            objData.Insert("GuardarEspecialidadMedico", CommandType.StoredProcedure, parameters);
        }

        public void EliminarEspecialidadMedico(Especialidad especialidad)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_medico", SqlDbType.Int, 4, especialidad.IdMedico),
                objData.CreateParameter("@i_id_especialidad", SqlDbType.Int, 4, especialidad.IdEspecialidad)
            };

            objData.Delete("EliminarEspecialidadMedico", CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region Paciente

        public Paciente ConsultarPaciente(int idTipoIdentificacion, string identificacion)
        {
            Paciente paciente = ConsultarDatosPaciente(idTipoIdentificacion, identificacion);

            return paciente;
        }

        public Paciente ConsultarDatosPaciente(int idTipoIdentificacion, string identificacion)
        {
            var objData = GetConnection();

            Paciente paciente = null;

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_tipo_identificacion", SqlDbType.Int, 4, idTipoIdentificacion),
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, identificacion),
                objData.CreateParameter("@i_id_tipo_persona", SqlDbType.Int, 4, 6)
            };

            var pacientes = objData.ConsultarDatos<Paciente>("ConsultarPersona", parameters);

            if (pacientes != null && pacientes.Count() > 0)
                paciente = pacientes.FirstOrDefault();

            return paciente;
        }

        public void GuardarPaciente(Paciente paciente)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                if (paciente.IdPersona == 0)
                    paciente.IdPersona = GuardarPersona(paciente);
                else
                    ActualizarPersona(paciente);

                GuardarTipoPersona(new TipoPersona() { IdPersona = paciente.IdPersona, IdTipo = 6 });

                tran.Complete();
            }
        }

        public void ActualizarPaciente(Paciente paciente)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                ActualizarPersona(paciente);

                var tipos = ConsultarTiposPersona(paciente.IdPersona);
                var tipoPaciente = tipos.FirstOrDefault(t => t.IdTipo == 6);

                if (!tipoPaciente.Estado && paciente.EstadoTipo)
                    GuardarTipoPersona(new TipoPersona() { IdPersona = paciente.IdPersona, IdTipo = 6 });

                tran.Complete();
            }
        }

        public void EliminarPaciente(Paciente paciente)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                EliminarTipoPersona(new TipoPersona() { IdPersona = paciente.IdPersona, IdTipo = 6 });

                tran.Complete();
            }
        }

        #endregion

        #region Especialidades

        public List<Especialidad> ConsultarEspecialidades()
        {
            var objData = GetConnection();

            var especialidades = objData.ConsultarDatos<Especialidad>("ConsultarEspecialidades");

            return especialidades;
        }

        #endregion
    }
}