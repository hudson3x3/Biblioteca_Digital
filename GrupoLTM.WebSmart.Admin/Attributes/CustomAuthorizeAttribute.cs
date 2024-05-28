using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Services.Log;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GrupoLTM.WebSmart.Admin.Attributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly List<EnumDomain.Perfis> _perfilList = new List<EnumDomain.Perfis>();

        public CustomAuthorizeAttribute(EnumDomain.Perfis _perfil)
        {
            _perfilList.Add(_perfil);
        }

        public CustomAuthorizeAttribute(params EnumDomain.Perfis[] _perfis)
        {
            foreach (var perfil in _perfis)
                _perfilList.Add(perfil);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                filterContext.Result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { { "action", "Index" }, { "controller", "Login" } });
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (_perfilList != null && _perfilList.Count > 0)
                {
                    var usuarioLogado = LoginHelper.GetLoginModel();

                    if (usuarioLogado == null)
                        return false;

                    if (!_perfilList.Contains((EnumDomain.Perfis)usuarioLogado.PerfilId))
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Error = ex.Message,
                    Date = DateTime.Now,
                    Class = "CustomAuthorizeAttribute",
                    Method = "AuthorizeCore",
                    StackTrace = ex.StackTrace,
                    Source = ex.ToString()
                };

                //TODO: Update DataDog
                //GrayLogServiceDesativado.LogError(log);

                return false;
            }
        }
    }
}