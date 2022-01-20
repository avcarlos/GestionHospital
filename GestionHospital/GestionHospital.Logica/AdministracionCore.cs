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

            var detalles = objData.ConsultarDatos<DetalleCatalogo>("ConsultarUsuario", parameters);

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

            var detalles = objData.ConsultarDatos<DetalleCatalogo>("ConsultarUsuario", parameters);

            if (detalles != null && detalles.Count() > 0)
                detalle = detalles.FirstOrDefault();

            return detalle;
        }

        #endregion

        #region Persona

        public Persona ConsultarPersona(int idTipoIdentificacion, string identificacion)
        {
            var objData = GetConnection();

            Persona persona = null;

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_tipo_identificacion", SqlDbType.Int, 4, idTipoIdentificacion),
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, identificacion)
            };

            var personas = objData.ConsultarDatos<Persona>("ConsultarPersona", parameters);

            if (personas != null && personas.Count() > 0)
                persona = personas.FirstOrDefault();

            return persona;
        }

        public void GuardarPersona(Persona persona)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[11]
            {
                objData.CreateParameter("@i_identificacion", SqlDbType.VarChar, 13, persona.Identificacion),
                objData.CreateParameter("@i_id_tipo_identicacion", SqlDbType.Int, 4, persona.IdTipoIdentificacion),
                objData.CreateParameter("@i_nombres", SqlDbType.VarChar, 50, persona.Nombres),
                objData.CreateParameter("@i_apellidos", SqlDbType.VarChar, 50, persona.Apellidos),
                objData.CreateParameter("@i_fecha_nacimiento", SqlDbType.DateTime, 8),
                objData.CreateParameter("@i_telefono", SqlDbType.VarChar, 12),
                objData.CreateParameter("@i_celular", SqlDbType.VarChar, 12),
                objData.CreateParameter("@i_email", SqlDbType.VarChar, 30),
                objData.CreateParameter("@i_id_genero", SqlDbType.Int, 4),
                objData.CreateParameter("@i_direccion", SqlDbType.VarChar, 300),
                objData.CreateParameter("@i_id_ciudad", SqlDbType.Int, 4)
            };

            if(persona.FechaNacimiento != null)
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

            objData.Insert("GuardarPersona", CommandType.StoredProcedure, parameters);
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
                    objData.CreateParameter("@i_email", SqlDbType.Int, 30),
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

        #endregion

        #region Medico

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