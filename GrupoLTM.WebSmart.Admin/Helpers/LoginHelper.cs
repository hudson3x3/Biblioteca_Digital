using System;
using System.Web;
using GrupoLTM.WebSmart.Admin.Models;
using GrupoLTM.WebSmart.Infrastructure.Cripto;
using Newtonsoft.Json;
using GrupoLTM.WebSmart.Admin.Facade;
using GrupoLTM.WebSmart.Services.Log;

namespace GrupoLTM.WebSmart.Admin.Helpers
{
    public class LoginHelper
    {
        public static bool IsLoggedOn()
        {
            return CookieManager.CurrentParticipanteModel != null;
        }

        public static bool AlterarSenha()
        {
            return CookieManager.CurrentParticipanteModel?.EmailRecuperacaoEnviado ?? false;
        }

        public static void LogOff()
        {
            new CookieManager().Remove("admUserKey");
            HttpResponse.RemoveOutputCacheItem("/Home/PartialMenuAdmin");
        }

        public static LoginModel GetLoginModel()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies.Get("admUserKey") == null)
                    return null;

                var httpCookie = HttpContext.Current.Request.Cookies.Get("admUserKey");

                if (httpCookie != null)
                {
                    var jsonCookie = AES.Decrypt256(Convert.FromBase64String(httpCookie.Value));
                    return JsonConvert.DeserializeObject<LoginModel>(jsonCookie);
                }

                return null;
            }
            catch (Exception ex)
            {
                var log = new LogControllerModel
                {
                    Error = ex.Message,
                    Date = DateTime.Now,
                    Class = "LoginHelper",
                    Method = "GetLoginModel",
                    StackTrace = ex.StackTrace,
                    Source = ex.ToString()
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return null;
            }
        }

        public static void SetLoginModel(LoginModel loginModel)
        {
            CookieManager cookieManager = new CookieManager();
            cookieManager.Set("admUserKey", AES.Encrypt256(JsonConvert.SerializeObject(loginModel)), 60);
        }
    }
}