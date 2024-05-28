using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Services;
using GrupoLTM.WebSmart.Infrastructure.Cripto;
using System.Configuration;
using System.Data;
using GrupoLTM.WebSmart.Services.JwtAuth;
using GrupoLTM.WebSmart.Services.Model;
using GrupoLTM.WebSmart.Services.Log;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Admin.Helpers;
using GrupoLTM.WebSmart.Infrastructure.Helpers;
using GrupoLTM.WebSmart.Domain.Repository;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    public class LoginController : BaseController
    {
        #region "Services"

        private readonly UsuarioAdminService _usuarioAdminService = new UsuarioAdminService();
        private readonly CatalogoService _catalogoService = new CatalogoService();
        private readonly ParticipanteService _participanteService = new ParticipanteService();
        private readonly JwtService _jwtService = new JwtService();

        #endregion

        #region "Actions"

        public ActionResult Index(string returnUrl)
        {
            ViewBag.EnableCaptcha = Convert.ToBoolean(ConfigurationManager.AppSettings["enable-recaptcha"]);
            ViewBag.SiteKey = ConfigurationManager.AppSettings["site-key-recaptcha"];

            if (!string.IsNullOrWhiteSpace(returnUrl))
                ViewBag.returnUrl = returnUrl;

            var loginModel = new LoginModel();

            CarregaConfiguracaoCampanha(loginModel);

            return View(loginModel);
        }

        public ActionResult LogOff()
        {
            LoginHelper.LogOff();
            return RedirectToAction("Index", "Login");
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador, EnumDomain.Perfis.Aluno)]
        [HttpGet]
        public ActionResult LogarComo()
        {
            return View(GetCatalogos(true));
        }

        [HttpGet]
        [CustomAuthorize(EnumDomain.Perfis.Administrador, EnumDomain.Perfis.Aluno)]
        public JsonResult BuscarPontuacaoParticipante(string accountNumber)
        {
            var userAdmin = LoginHelper.GetLoginModel();

            try
            {
                var extrato = _participanteService.BuscarPontuacaoParticipante(accountNumber);

                return Json(new { Sucesso = true, extrato }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Method = "POST",
                    Action = "BuscarPontuacaoParticipante",
                    Controller = "LoginController",
                    Message = "Erro ao buscar a pontuação do participante",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    AccountNumber = accountNumber,
                    UserAdminId = userAdmin.Id,
                    UserAdminLogin = userAdmin.Login,
                    IP = HttpContext.Request.UserHostAddress.ToString(),
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador, EnumDomain.Perfis.Aluno)]
        [HttpGet]
        public void RedirectAvon(string avonAccount)
        {
            var userAdmin = LoginHelper.GetLoginModel();

            try
            {
                var avonAccountData = avonAccount.Split(';');
                var catalogoId = Convert.ToInt32(avonAccountData[0]);
                var accountNumber = avonAccountData[1];
                var accountName = avonAccountData[2];
                var pageName = avonAccountData[3];
                var numberCampaign = avonAccountData[4];
                var yearCampaign = avonAccountData[5];
                var accountType = avonAccountData[6];

                var preview = avonAccountData.Length >= 8 ? Convert.ToBoolean(avonAccountData[7]) : false;

                var urlRedirect = string.Empty;

                if (pageName == "catalogo")
                {
                    var _jwtOptions = new JwtOptions()
                    {
                        ValidIssuer = ConfigurationManager.AppSettings["SsoValidIssuerAdmin"],
                        SecurityKey = ConfigurationManager.AppSettings["SsoSecurityKeyAdmin"],
                        Audience = ConfigurationManager.AppSettings["SsoAudienceAdmin"],
                        SsoEndpoint = ConfigurationManager.AppSettings["SsoEndpointAdmin"],
                        additionalinfo = accountType,
                        Sub = accountNumber
                    };

                    var jwt = _jwtService.GenerateJwt(_jwtOptions);

                    urlRedirect = jwt;
                }
                else
                {
                    urlRedirect = GetAccessLink(catalogoId, accountNumber, pageName, numberCampaign, yearCampaign, userAdmin.Id, preview);
                }

                Response.Redirect(urlRedirect);

                var log = new LogControllerModel
                {
                    Method = "GET",
                    Action = "RedirectAvon",
                    Controller = "LoginController",
                    Message = "Redirecionamento de participante - " + pageName,
                    Page = pageName,
                    AccountNumber = accountNumber,
                    UrlRedirect = urlRedirect,
                    Info = avonAccount,
                    UserAdminId = userAdmin.Id,
                    UserAdminLogin = userAdmin.Login,
                    IP = HttpContext.Request.UserHostAddress.ToString(),
                };

                //TODO: Update DataDog
                //GrayLogService.Log(log);
            }
            catch (Exception ex)
            {
                var data = avonAccount.Split(';');
                var accountNumber = data.Length > 1 ? data[1] : "Não identificado";

                var log = new LogControllerModel
                {
                    Method = "GET",
                    Action = "RedirectAvon",
                    Controller = "LoginController",
                    Message = "Erro ao redirecionar o participante",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    AccountNumber = accountNumber,
                    Info = avonAccount,
                    UserAdminId = userAdmin.Id,
                    UserAdminLogin = userAdmin.Login,
                    IP = HttpContext.Request.UserHostAddress.ToString(),
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);
            }
        }

        #endregion

        #region "Actions Posts"

        [HttpPost]
        [PreventSpam]
        [ValidateCaptcha]
        public ActionResult LogarUsuario(string login, string senha, string returnUrl)
        {
            try
            {
                var usuario = _usuarioAdminService.Login(login, senha);

                var loginModel = new LoginModel
                {
                    Id = usuario.Id,
                    PerfilId = usuario.PerfilId,
                    Login = usuario.Login,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    EmailRecuperacaoEnviado = usuario.EmailRecuperacaoEnviado
                };

                CarregaConfiguracaoCampanha(loginModel);

                LoginHelper.SetLoginModel(loginModel);

                return Json(new { ok = true, message = string.Empty, returnUrl }, JsonRequestBehavior.AllowGet);
            }
            catch (SistemaException ex)
            {
                LogException(ex, login);

                var mensagem = "Login e/ou senha inválidos, caso haja três ou mais tentativas, o acesso será bloqueado.";

                return Json(new { ok = false, msg = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogException(ex, login);

                return Json(new { ok = false, msg = "Houve um erro ao autenticar o usuário" }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize(EnumDomain.Perfis.Administrador, EnumDomain.Perfis.Aluno)]
        [HttpPost]
        public JsonResult GetAccountAvon(string accountNumber)
        {
            var userAdmin = LoginHelper.GetLoginModel();

            try
            {
                var userAvon = AvonService.GetNewUserInfo(Convert.ToInt64(accountNumber), "Admin-GetAccountAvon-" + userAdmin.Login);

                if (userAvon.ErrorMessage != null)
                    return Json(new { Sucesso = false, Mensagem = userAvon.ErrorMessage });

                var invalidEmail = false;
                var msgAtualizacao = string.Empty;
                var msgEmail = string.Empty;

                try
                {
                    invalidEmail = AvonService.AtualizarParticipanteMarketplace(userAvon, accountNumber, userAdmin.Id);

                    if (invalidEmail)
                        msgEmail = "EmailInvalidoParticipante: " + userAvon.profileAvon.emailAddrTxt;
                }
                catch (Exception ex)
                {
                    msgAtualizacao = "Não foi possível atualizar o participante no Marketplace: " + ex.Message;
                }

                var log = new LogLogarComoModel
                {
                    AccountNumber = accountNumber,
                    Message = "Obter participante Avon",
                    NameAccountNumber = userAvon.profileAvon?.frstNm + " " + userAvon.profileAvon?.lastNm,
                    Catalogo = userAvon.profileAvon?.repPrftblty?.repPrftbltyTypDescTxt,
                    IP = HttpContext.Request.UserHostAddress.ToString(),
                    UserAdminId = userAdmin.Id,
                    UserAdminLogin = userAdmin.Login,
                    UserInfoAvon = userAvon,
                    EmailInvalido = invalidEmail,
                    TextoEmailInvalido = msgEmail,
                    ErroAtualizacao = msgAtualizacao,
                };

                //TODO: Update DataDog
                //GrayLogService.Log(log);

                return Json(new { Sucesso = true, Retorno = new { userAvon.profileAvon, invalidEmail, msgAtualizacao } });
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Method = "POST",
                    Action = "GetAccountAvon",
                    Controller = "LoginController",
                    Message = "Erro ao buscar o participante",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    AccountNumber = accountNumber,
                    UserAdminId = userAdmin.Id,
                    UserAdminLogin = userAdmin.Login,
                    IP = HttpContext.Request.UserHostAddress.ToString(),
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        #endregion

        #region "Internal Functions"

        private SelectList GetCatalogos(bool inDate = false)
        {
            List<SelectListItem> itens = new List<SelectListItem>();
            List<DTO.CatalogoModel> retorno = new List<DTO.CatalogoModel>();

            using (var context = UnitOfWorkFactory.Create())
            {
                var repUsuario = context.CreateRepository<UsuarioAdm>();
                var usuario = repUsuario.Find<UsuarioAdm>(LoginHelper.GetLoginModel().PerfilId);

                if (!inDate)
                    retorno = _catalogoService.ListarCatalogos(usuario.PerfilId).OrderBy(x => x.Id).ToList();
                else
                    retorno = _catalogoService.ListarCatalogos(usuario.PerfilId).Where(x => x.DataInclusao < DateTime.Now && x.DataAlteracao > DateTime.Now)
                        .OrderBy(x => x.Id).ToList();

                retorno.ForEach(catalogo =>
                {
                    itens.Add(new SelectListItem()
                    {
                        Value = catalogo.Codigo.ToString(),
                        Text = catalogo.Nome
                    });
                });

                return new SelectList(retorno, "MktPlaceCatalogoId", "Nome");

            }

        }

        private string GetAccessLink(int catalogoId, string accountNumber, string pageName, string numberCampaign, string yearCampaign, int? adminUserId = null, bool preview = false)
        {
            var urlSiteAvon = ConfigurationManager.AppSettings["UrlSiteAvon"].ToString();

            var key = accountNumber + "AvonLtm";

            if (adminUserId > 0)
                key += adminUserId;

            var encriptedKey = AES.CalculateMD5Hash(key);

            var urlGerada = $"{urlSiteAvon}/avonAuthentication/{catalogoId}/{pageName}/?a={accountNumber}&encryptedKey={encriptedKey}&numberCampaign={numberCampaign}&yearCampaign={yearCampaign}&preview={preview}"; ;

            if (adminUserId > 0)
                urlGerada += $"&i={adminUserId}";

            return urlGerada;
        }

        private static void GravaLogErro(string Erro, string Mensagem, string Source, string Metodo, string Codigo)
        {
            var logErro = new LogErro()
            {
                Erro = Erro,
                Mensagem = Mensagem,
                Source = Source,
                Metodo = Metodo,
                Controller = "ArquivoService",
                Pagina = string.Empty,
                Codigo = Codigo
            };

            var logErroService = new LogErroService();
            logErroService.SalvarLogErro(logErro);
        }

        private static void GravaLogProcessamento(string Mensagem, string Source, string Metodo, string codigo)
        {
            Console.WriteLine(DateTime.Now.ToString() + " - " + Mensagem);
            var logProcessamento = new LogProcessamentoDb()
            {
                Metodo = Metodo,
                Controller = "LoginController",
                Codigo = codigo,
                Mensagem = Mensagem,
                Source = Source,
                DataInclusao = DateTime.Now
            };

            var logService = new LogService();
            logService.GravarLogProcessamento(logProcessamento);
        }

        private void LogException(Exception ex, string login)
        {
            var log = new LogControllerModel
            {
                Method = "HttpPost",
                Class = "LoginController",
                Message = "Erro ao realizar o login do usuário",
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
