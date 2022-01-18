using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private Usuario oUsuario;

        private int idOperacion;

        public AuthorizeUser(int idOperacion = 0)
        {
            this.idOperacion = idOperacion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                oUsuario = (Usuario)HttpContext.Current.Session["Usuario"];

                // TO DO - Validar operaciones del usuario
                //filterContext.Result = new RedirectResult("");
            }
            catch(Exception ex)
            {
                // Construir vista de errores
                //filterContext.Result = new RedirectResult("");
            }
        }
    }
}