using System;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Services.Log;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class EsqueciSenhaController : BaseController
    {
        private readonly UsuarioAdminService _usuarioAdminService = new UsuarioAdminService();

        #region "Actions"

        public ActionResult Index()
        {
            var loginModel = new Models.LoginModel();
            CarregaConfiguracaoCampanha(loginModel);
            return View(loginModel);
        }

        #endregion

        #region "Actions Posts"

        [HttpPost]
        [PreventSpam]
        public ActionResult EnviarSenha(string login)
        {
            var msgRetorno = "Solicitação de email requisitada";

            try
            {
                _usuarioAdminService.EnviarSenhaEmail(login);

                return Json(new { msg = msgRetorno }, JsonRequestBehavior.AllowGet);
            }
            catch (SistemaException ex)
            {
                LogException(ex, login);

                return Json(new { msg = msgRetorno }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogException(ex, login);

                return Json(new { msg = "Houve um erro ao enviar o email" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void LogException(Exception ex, string login)
        {
            var log = new LogControllerModel
            {
                Method = "HttpPost",
                Class = "EsqueciSenhaController",
                Message = "Erro ao enviar a senha",
                Error = ex.Message,
                StackTrace = ex.StackTrace,
                Login = login
            };

            //TODO: Update DataDog
            //GrayLogService.LogError(log);
        }

        #endregion
    }
}
