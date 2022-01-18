using GestionHospital.Controllers;
using GestionHospital.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionHospital.Filters
{
    public class VerificaSession : ActionFilterAttribute
    {
        private Usuario oUsuario;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                oUsuario = (Usuario)HttpContext.Current.Session["Usuario"];

                if (oUsuario == null)
                {
                    if (filterContext.Controller is AccountController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Account/Login");
                    }
                }
            }
            catch(Exception)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}