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
    public class SeguridadCore
    {
        private static DBManager GetConnection() => new DBManager();

        #region Usuario

        public Usuario ConsultarUsuario(string nombreUsuario)
        {
            var objData = GetConnection();

            Usuario usuario = null;

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_nombre_usuario", SqlDbType.VarChar, 30, nombreUsuario)
            };

            var usuarios = objData.ConsultarDatos<Usuario>("ConsultarUsuario", parameters);

            if (usuarios != null && usuarios.Count() > 0)
                usuario = usuarios.FirstOrDefault();

            return usuario;
        }

        public Usuario ConsultarUsuarioPersona(int idPersona)
        {
            var objData = GetConnection();

            Usuario usuario = null;

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, idPersona)
            };

            var usuarios = objData.ConsultarDatos<Usuario>("ConsultarUsuarioPersona", parameters);

            if (usuarios != null && usuarios.Count() > 0)
                usuario = usuarios.FirstOrDefault();

            return usuario;
        }

        public List<Usuario> ConsultarUsuarios()
        {
            var objData = GetConnection();

            var usuarios = objData.ConsultarDatos<Usuario>("ConsultarUsuario");

            return usuarios;
        }

        public List<Transaccion> ConsultarTransaccionesUsuario(string loginUsuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_nombre_usuario", SqlDbType.VarChar, 30, loginUsuario)
            };

            var transacciones = objData.ConsultarDatos<Transaccion>("ConsultarTransaccionesUsuario", parameters);

            return transacciones;
        }

        public int GuardarUsuario(Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[4]
            {
                objData.CreateParameter("@i_nombre_usuario", SqlDbType.VarChar, 30, usuario.LoginUsuario),
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, usuario.IdRolSeguridad),
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4),
                objData.CreateParameter("@o_id_usuario", SqlDbType.Int, 4, ParameterDirection.Output)
            };

            if (usuario.IdPersona != null)
                parameters[2].Value = usuario.IdPersona.GetValueOrDefault();

            int idUsuario = objData.Insert("GuardarUsuario", CommandType.StoredProcedure, parameters);
            
            return idUsuario;
        }

        public void RegistrarPersonaUsuario(Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario),
                objData.CreateParameter("@i_id_persona", SqlDbType.Int, 4, usuario.IdPersona.GetValueOrDefault())
            };

            if (usuario.IdPersona != null)
                parameters[0].Value = usuario.IdPersona.GetValueOrDefault();

            objData.Update("RegistrarPersonaUsuario", CommandType.StoredProcedure, parameters);
        }

        public string GenerarPasswordUsuario(Usuario usuario)
        {
            string nuevoPassword = Guid.NewGuid().ToString();

            nuevoPassword = char.ToUpper(usuario.LoginUsuario[0]) +
                            usuario.LoginUsuario.Substring(1, 3) + 
                            nuevoPassword.Substring(0, 3) + 
                            "." + usuario.IdUsuario.ToString();

            return nuevoPassword;
        }

        public bool ValidarExisteUsuario(string loginUsuario)
        {
            bool existe = false;

            var usuario = ConsultarUsuario(loginUsuario);

            if (usuario != null)
                existe = true;

            return existe;
        }

        #endregion
    }
}