using GestionHospital.DataAccess;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionHospital.Logica
{
    public class SeguridadCore
    {
        private static DBManager GetConnection() => new DBManager();

        public List<Usuario> ConsultarUsuarios()
        {
            var objData = GetConnection();

            var usuarios = objData.ConsultarDatos<Usuario>("ConsultarUsuario");

            return usuarios;
        }
    }
}