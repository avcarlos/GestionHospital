using GestionHospital.DataAccess;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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

        public bool ValidarPasswordUsuario(Usuario usuario, string password)
        {
            var objData = GetConnection();

            bool valido = false;

            IDbDataParameter[] parameters = new IDbDataParameter[3]
            {
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario),
                objData.CreateParameter("@i_password", SqlDbType.VarChar, 300, password),
                objData.CreateParameter("@o_valido", SqlDbType.Bit, 1, ParameterDirection.Output)
            };

            var retorno = objData.Execute("ValidarPasswordUsuario", CommandType.StoredProcedure, parameters);

            if (retorno != null && retorno.Count() > 0)
            {
                var datoRetorno = (SqlParameter)retorno[0];

                if (datoRetorno.SqlDbType == SqlDbType.Bit)
                    valido = (bool)datoRetorno.Value;
            }

            return valido;
        }

        #endregion

        #region Transacciones

        public List<Transaccion> ConsultarTransacciones(bool incluirInactivas = false)
        {
            var objData = GetConnection();

            var transacciones = objData.ConsultarDatos<Transaccion>("ConsultarTransacciones");

            if (!incluirInactivas)
                transacciones = transacciones.FindAll(t => t.Estado);

            return transacciones;
        }

        public void ActualizarTransaccion(Transaccion transaccion, Usuario usuario)
        {
            if (transaccion != null)
            {
                var objData = GetConnection();

                IDbDataParameter[] parameters = new IDbDataParameter[4]
                {
                    objData.CreateParameter("@i_id_transaccion", SqlDbType.Int, 4, transaccion.IdTransaccion),
                    objData.CreateParameter("@i_descripcion", SqlDbType.VarChar, 300, transaccion.Descripcion),
                    objData.CreateParameter("@i_estado", SqlDbType.Bit, 1, transaccion.Estado),
                    objData.CreateParameter("@i_id_usuario_modificacion", SqlDbType.Int, 4, usuario.IdUsuario)
                };

                objData.Update("ActualizarTransaccion", CommandType.StoredProcedure, parameters);
            }
        }

        public void EliminarTransaccion(Transaccion transaccion, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_transaccion", SqlDbType.Int, 4, transaccion.IdTransaccion),
                objData.CreateParameter("@i_id_usuario_modificacion", SqlDbType.Int, 4, usuario.IdUsuario)
            };

            objData.Delete("EliminarTransaccion", CommandType.StoredProcedure, parameters);
        }

        #endregion

        #region Roles de Seguridad

        public List<RolSeguridad> ConsultarRolesSeguridad(bool incluirInactivas = false)
        {
            var objData = GetConnection();

            var roles = objData.ConsultarDatos<RolSeguridad>("ConsultarRolesSeguridad");

            if (!incluirInactivas)
                roles = roles.FindAll(e => e.Estado);

            return roles;
        }

        public int GuardarRolSeguridad(RolSeguridad rol, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[4]
            {
                objData.CreateParameter("@i_nombre", SqlDbType.VarChar, 30, rol.Nombre),
                objData.CreateParameter("@i_descripcion", SqlDbType.VarChar, 300, rol.Descripcion),
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario),
                objData.CreateParameter("@o_id_rol_seguridad", SqlDbType.Int, 4, ParameterDirection.Output)
            };

            var idRol = objData.Insert("GuardarRolSeguridad", CommandType.StoredProcedure, parameters);

            return idRol;
        }

        public void GuardarRol(RolSeguridad rol, Usuario usuario)
        {
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                var idRol = GuardarRolSeguridad(rol, usuario);

                if (rol.Transacciones != null && rol.Transacciones.Count() > 0)
                {
                    foreach (var item in rol.Transacciones)
                    {
                        GuardarTransaccionRolSeguridad(idRol, item.IdTransaccion);
                    }
                }

                tran.Complete();
            }
        }

        public void ActualizarRolSeguridad(RolSeguridad rolSeguridad, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[5]
            {
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, rolSeguridad.IdRolSeguridad),
                objData.CreateParameter("@i_nombre", SqlDbType.VarChar, 30, rolSeguridad.Nombre),
                objData.CreateParameter("@i_descripcion", SqlDbType.VarChar, 300, rolSeguridad.Descripcion),
                objData.CreateParameter("@i_estado", SqlDbType.Bit, 1, rolSeguridad.Estado),
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario)
            };

            objData.Update("ActualizarRolSeguridad", CommandType.StoredProcedure, parameters);
        }

        public void ActualizarRol(RolSeguridad rol, Usuario usuario)
        {
            if (rol.Transacciones == null)
                rol.Transacciones = new List<Transaccion>();

            var transaccionesIniciales = ConsultarTransaccionesRolSeguridad(rol.IdRolSeguridad);

            var transaccionesGuardar = rol.Transacciones.FindAll(e => !transaccionesIniciales.Exists(i => i.IdTransaccion == e.IdTransaccion));
            var transaccionesEliminar = transaccionesIniciales.FindAll(i => !rol.Transacciones.Exists(e => e.IdTransaccion == i.IdTransaccion));

            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
            {
                ActualizarRolSeguridad(rol, usuario);

                if (transaccionesGuardar != null && transaccionesGuardar.Count() > 0)
                {
                    foreach (var item in transaccionesGuardar)
                    {
                        GuardarTransaccionRolSeguridad(rol.IdRolSeguridad, item.IdTransaccion);
                    }
                }

                if (transaccionesEliminar != null && transaccionesEliminar.Count() > 0)
                {
                    foreach (var item in transaccionesEliminar)
                    {
                        EliminarTransaccionRolSeguridad(item.IdTransaccionRolSeguridad);
                    }
                }

                tran.Complete();
            }
        }

        public void EliminarRolSeguridad(RolSeguridad rolSeguridad, Usuario usuario)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, rolSeguridad.IdRolSeguridad),
                objData.CreateParameter("@i_id_usuario", SqlDbType.Int, 4, usuario.IdUsuario)
            };

            objData.Delete("EliminarRolSeguridad", CommandType.StoredProcedure, parameters);
        }

        public List<Transaccion> ConsultarTransaccionesRolSeguridad(int idRolSeguridad)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, idRolSeguridad)
            };

            var transacciones = objData.ConsultarDatos<Transaccion>("ConsultarTransaccionesRolSeguridad", parameters);

            return transacciones;
        }

        public void GuardarTransaccionRolSeguridad(int idRolSeguridad, int idTransaccion)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[2]
            {
                objData.CreateParameter("@i_id_rol_seguridad", SqlDbType.Int, 4, idRolSeguridad),
                objData.CreateParameter("@i_id_transaccion", SqlDbType.Int, 4, idTransaccion)
            };

            objData.Insert("GuardarTransaccionRolSeguridad", CommandType.StoredProcedure, parameters);
        }

        public void EliminarTransaccionRolSeguridad(int idTransaccionRolSeguridad)
        {
            var objData = GetConnection();

            IDbDataParameter[] parameters = new IDbDataParameter[1]
            {
                objData.CreateParameter("@i_id_transaccion_rol_seguridad", SqlDbType.Int, 4, idTransaccionRolSeguridad)
            };

            objData.Delete("EliminarTransaccionRolSeguridad", CommandType.StoredProcedure, parameters);
        }

        #endregion
    }
}