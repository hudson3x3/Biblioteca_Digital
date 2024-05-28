using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Security.Cryptography;
using GrupoLTM.WebSmart.DTO;
using GrupoLTM.WebSmart.Services.Log;
using GrupoLTM.WebSmart.Infrastructure.Cache;
using System.Threading.Tasks;

namespace GrupoLTM.WebSmart.Admin.Attributes
{
    public class PreventSpamAttribute : ActionFilterAttribute
    {
        private readonly int delayRequest;

        private readonly string errorMessage = "Excessive Request Attempts Detected";

        private readonly RedisService _redisService;

        public PreventSpamAttribute()
        {
            _redisService = new RedisService();
            delayRequest = Convert.ToInt32(ConfigurationManager.AppSettings["delayRequestSeconds"]);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isSpam = CheckSpam(filterContext.HttpContext.Request);

            if (isSpam)
            {
                filterContext.HttpContext.Response.Write(errorMessage);
                filterContext.HttpContext.Response.StatusDescription = errorMessage;
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                filterContext.Result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private bool CheckSpam(HttpRequestBase request)
        {
            try
            {
                var userIp = request.UserHostAddress;

                if (string.IsNullOrEmpty(userIp))
                {
                    userIp = request.ServerVariables["REMOTE_ADDR"];

                    if (string.IsNullOrEmpty(userIp))
                        userIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }

                var originationInfo = userIp + request.UserAgent;

                var targetInfo = request.RawUrl + request.QueryString;

                var cacheKey = string.Join(string.Empty, MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(originationInfo + targetInfo)).Select(s => s.ToString("x2")));

                var cacheValue = _redisService.GetCache(cacheKey);

                var hasValue = !string.IsNullOrEmpty(cacheValue);

                if (hasValue)
                    LogSpan(request, userIp);

                _redisService.SetCache(cacheKey, cacheKey, delayRequest);

                return hasValue;
            }
            catch (Exception ex)
            {
                var log = new LogGrayLog
                {
                    Method = "CheckSpam",
                    Class = "PreventSpamAttribute",
                    Message = "Erro ao gravar o cache de spam",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                };

                //TODO: Update DataDog
                //GrayLogService.LogError(log);

                return false;
            }
        }

        private void LogRequest(HttpRequestBase request, string ip)
        {
            Task.Run(() =>
            {
                var action = request.RequestContext.RouteData.Values["action"].ToString();
                var controller = request.RequestContext.RouteData.Values["controller"].ToString();

                var log = new LogControllerModel
                {
                    IP = ip,
                    Action = action,
                    Controller = controller,
                    Class = "PreventSpamAttribute",
                    Method = "CheckSpam",
                    Message = "Informações do request",
                    Data = new
                    {
                        request.RawUrl,
                        request.UserAgent,
                        request.QueryString,
                        RemoteAddress = request.ServerVariables["REMOTE_ADDR"],
                        HttpForwardedFor = request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                    }
                };

                //TODO: Update DataDog
                //GrayLogService.Log(log);
            });
        }

        private void LogSpan(HttpRequestBase request, string ip)
        {
            Task.Run(() =>
            {
                var action = request.RequestContext.RouteData.Values["action"].ToString();
                var controller = request.RequestContext.RouteData.Values["controller"].ToString();

                var log = new LogSpanModel
                {
                    IP = ip,
                    Action = action,
                    Controller = controller,
                    Class = "PreventSpamAttribute",
                    Method = "CheckSpam",
                    Message = errorMessage
                };

                //TODO: Update DataDog
                //GrayLogService.Log(log);
            });
        }
    }
}