using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Repository;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Admin.Facade;
using System.Net;
using GrupoLTM.WebSmart.Services.Log;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class BaseController : Controller
    {
        #region "Services"

        private readonly CookieManager _cookieManager = new CookieManager();
        private readonly CatalogoService _catalogoService = new CatalogoService();

        #endregion

        protected string echo, sSearch, sSortDir_0;
        protected int iSortCol_0 = 0, regExibir = 0, startExibir = 0;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var controllersDeslogadas = new string[] { "Login", "EsqueciSenha" };

                var controllerRequest = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                var contemAuth = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CustomAuthorizeAttribute), true).Any();

                if (controllersDeslogadas.Contains(controllerRequest) && !contemAuth)
                    return;

                if (!LoginHelper.IsLoggedOn())
                {
                    var url = new UrlHelper(filterContext.RequestContext);

                    var returnUrl = "~/Login/Index?returnUrl=" + filterContext.HttpContext.Request.Url.PathAndQuery;

                    var loginUrl = url.Content(returnUrl);

                    _cookieManager.Remove("admUserKey");

                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        filterContext.HttpContext.Response.End();
                    }
                    else
                    {
                        filterContext.HttpContext.Response.Redirect(loginUrl);
                    }
                }
                else
                {
                    if (controllerRequest != "AlterarSenha" && LoginHelper.AlterarSenha())
                    {
                        var url = new UrlHelper(filterContext.RequestContext);

                        var alterarSenhaUrl = url.Content("~/AlterarSenha/Index");

                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                            filterContext.HttpContext.Response.End();
                        else
                            filterContext.HttpContext.Response.Redirect(alterarSenhaUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Error = ex.Message,
                    Date = DateTime.Now,
                    Class = "BaseController",
                    Method = "OnActionExecuting",
                    StackTrace = ex.StackTrace,
                    Source = ex.ToString()
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);
            }

            base.OnActionExecuting(filterContext);
        }

        protected class DataTableRetorno<T>
        {
            public int iTotalRecords { get; set; }
            public int iTotalDisplayRecords { get; set; }
            public string sEcho { get; set; }
            public List<T> aaData { get; set; }
        }

        #region "Public functions"

        public static void CarregaConfiguracaoCampanha(LoginModel loginModel)
        {
            using (IUnitOfWork context = UnitOfWorkFactory.Create())
            {
                IRepository repConfiguracaoCampanha = context.CreateRepository<ConfiguracaoCampanha>();
                var configuracaoCampanha = repConfiguracaoCampanha.All<ConfiguracaoCampanha>().FirstOrDefault();
                ConfiguracaoModel configuracaoCampanhaModel = new ConfiguracaoModel();

                if (configuracaoCampanha != null)
                {
                    loginModel.NomeCampanha = configuracaoCampanha.NomeCampanha;
                    loginModel.TipoAcessoId = configuracaoCampanha.TipoAcessoId;
                    loginModel.TipoCadastroId = configuracaoCampanha.TipoCadastroId;
                    loginModel.TipoValidacaoPositivaId = configuracaoCampanha.TipoValidacaoPositivaId;
                    loginModel.Logo = configuracaoCampanha.ImgLogoCampanha;
                    loginModel.AtivoBoxSaldo = configuracaoCampanha.AtivoBoxSaldo;
                    loginModel.AtivoBoxVitrine = configuracaoCampanha.AtivoBoxVitrine;
                    loginModel.AtivoWP = configuracaoCampanha.AtivoWP;
                }
            }
        }

        //public static void GravarLog(ActionExecutingContext filterContext)
        //{
        //    //Grava Log
        //    int IdUsuario = ((LoginHelper.GetLoginModel() == null) ? 0 : LoginHelper.GetLoginModel().Id);
        //    LogService.CadastrarLog(
        //        IdUsuario,
        //        //filterContext.HttpContext.Request.Form.ToString(),
        //        String.Empty,
        //        filterContext.HttpContext.Request.UserHostAddress.ToString(),
        //        filterContext.ActionDescriptor.ActionName,
        //        filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
        //        String.Empty,
        //        filterContext.HttpContext.Request.Url.Host,
        //        String.Empty,
        //        filterContext.HttpContext.Request.Browser.Browser.ToString(),
        //        filterContext.HttpContext.Request.Browser.Version.ToString(),
        //        filterContext.HttpContext.Request.Url.AbsolutePath,
        //        string.Empty,
        //        filterContext.HttpContext.Request.RequestType,
        //        string.Empty,
        //        false);
        //}

        #endregion

        #region "Métodos Privados"

        protected void inicializaDadosDatatable()
        {
            echo = Request.Params["sEcho"].ToString();
            sSearch = Request.Params["sSearch"].ToString();
            sSortDir_0 = Request.Params["sSortDir_0"] != null ? Request.Params["sSortDir_0"].ToString() : "asc";
            iSortCol_0 = 0;
            regExibir = 0;
            startExibir = 0;

            Int32.TryParse(Request.Params["iSortCol_0"], out iSortCol_0);
            Int32.TryParse(Request.Params["iDisplayLength"], out regExibir);
            Int32.TryParse(Request.Params["iDisplayStart"], out startExibir);
        }

        protected SelectList getCatalogos(bool isMarketPlaceIdReturned = false)
        {
            List<SelectListItem> itens = new List<SelectListItem>();

            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                var retorno = _catalogoService.ListarCatalogos(usuario.PerfilId).Where(x => x.Id != 1).OrderBy(x => x.Id).ToList();

                return isMarketPlaceIdReturned ? new SelectList(retorno, "MktPlaceCatalogoId", "Nome") : new SelectList(retorno, "Id", "Nome");

            }
        }

        #endregion

    }
}
