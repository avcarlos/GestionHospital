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

        public void GuardarUsuario(Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_nombre_usuario", SqlDbType.VarChar, 30, usuario.LoginUsuario),
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, usuario.IdRolSeguridad),
                objData.CreateParameter("@o_id_usuario", SqlDbType.Int, 4, ParameterDirection.Output)
            };

            objData.Insert("GuardarUsuario", CommandType.StoredProcedure, parameters);
        }
    }
}