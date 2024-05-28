using GrupoLTM.WebSmart.Domain.Models;
using GrupoLTM.WebSmart.Admin.Attributes;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Services;
using System.Configuration;
using System.Web.Mvc;
using System.Web;
using System;
using GrupoLTM.WebSmart.Services.Log;
using GrupoLTM.WebSmart.Domain.Enums;
using GrupoLTM.WebSmart.Admin.Helpers;

namespace GrupoLTM.WebSmart.Admin.Controllers
{
    [CustomAuthorize(EnumDomain.Perfis.Administrador)]
    public class BannersController : BaseController
    {
        private readonly BannerService bannersService = new BannerService();

        [HttpGet]
        public ActionResult Index()
        {
            var urlBlob = ConfigurationManager.AppSettings["Storage.Banners"];

            var accountNumber = ConfigurationManager.AppSettings["DefaultAccountNumber"];

            var banners = bannersService.ObterBanners();

            var bannerModel = new BannersModel(banners, urlBlob);

            SetHeadersRequest(accountNumber);

            return View(bannerModel);
        }

        [HttpPost]
        public string Salvar(BannersModel model)
        {
            var usuario = LoginHelper.GetLoginModel();

            if (usuario == null)
                return "invalid token";

            try
            {
                bannersService.AtualizarBanners(model.Banners, usuario.Id);

                return UrlRedirect(false);
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Method = "Salvar",
                    Class = "BannersController",
                    Message = "Erro ao publicar os banners",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return "error";
            }
        }

        [HttpPost]
        public string SalvarPreview(BannersModel model)
        {
            try
            {
                var usuario = LoginHelper.GetLoginModel();

                if (usuario == null)
                    return "invalid token";

                bannersService.CadastrarBannersPreview(model.Banners, usuario.Id);

                return UrlRedirect(true);
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Method = "SalvarPreview",
                    Class = "BannersController",
                    Message = "Erro ao visualizar a preview dos banners",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return "error";
            }
        }

        private string UrlRedirect(bool preview)
        {
            var accountNumber = ConfigurationManager.AppSettings["DefaultAccountNumber"];

            var userAvon = AvonService.GetNewUserInfo(Convert.ToInt64(accountNumber), "Admin-Banners");

            var pageName = "hotsite";
            var accountName = userAvon.profileAvon.frstNm + " " + userAvon.profileAvon.lastNm;
            var numberCampaign = userAvon.profileAvon.currSlsCmpgnNr;
            var yearCampaign = userAvon.profileAvon.currSlsYrNr;

            var url = $"0;{accountNumber};{accountName};{pageName};{numberCampaign};{yearCampaign};" + preview;

            return url;
        }

        private void SetHeadersRequest(string accountNumber)
        {
            var tokenApi = GerarToken(accountNumber);

            var urlApi = ConfigurationManager.AppSettings["apiAvon_url"];
            var subscription = ConfigurationManager.AppSettings["apiMan_subscriptionKey"];

            var urlCatalogo = ConfigurationManager.AppSettings["urlCatalogo"];
            var siteAvonUrl = ConfigurationManager.AppSettings["urlAvon"];
            var avonComigoUrl = ConfigurationManager.AppSettings["urlAvonComigo"];

            var headers = new
            {
                urlApi = urlApi,
                subs = subscription,
                auth = "bearer " + tokenApi,
                login = accountNumber,
                catalogo = urlCatalogo,
                siteAvon = siteAvonUrl,
                avonComigo = avonComigoUrl,
            };

            ViewBag.headers = headers;
        }

        private string GerarToken(string accountNumber)
        {
            var avonAuthentication = new AvonAuthentication
            {
                AccountNumber = accountNumber,
                PageName = "hotsite",
                CatalogId = 1,
                i = 0
            };

            string tokenApi;

            var base64Server = string.Format("{0}{1}{2}{3}{4}{5}",
                        avonAuthentication.AccountNumber,
                        avonAuthentication.CatalogId.ToString(),
                        avonAuthentication.PageName,
                        avonAuthentication.i == null ? 0 : avonAuthentication.i,
                        avonAuthentication.numberCampaign,
                        avonAuthentication.yearCampaign);

            if (Request.Cookies["tokenCookie"] == null || Request.Cookies["base64Browser"] == null)
                tokenApi = GetToken(avonAuthentication, base64Server);
            else
            {
                if (Request.Cookies["base64Browser"].Value == base64Server)
                    tokenApi = Request.Cookies["tokenCookie"].Value;
                else
                    tokenApi = GetToken(avonAuthentication, base64Server);
            }

            return tokenApi;
        }

        private string GetToken(AvonAuthentication avonAuthentication, string base64Server)
        {
            var token = OAuthService.GetToken(avonAuthentication);

            //Gerenciamento de Cookie
            var now = DateTime.Now;

            var tokenCookie = new HttpCookie("tokenCookie")
            {
                Value = token,
                Expires = now.AddHours(2)
            };

            var base64BrowserCookie = new HttpCookie("base64Browser")
            {
                Value = base64Server,
                Expires = now.AddHours(2)
            };

            Response.SetCookie(tokenCookie);
            Response.SetCookie(base64BrowserCookie);

            return token;
        }
    }
}