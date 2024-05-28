using System;
using System.Net;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GrupoLTM.WebSmart.Admin.Attributes
{
    public class ValidateCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var enabled = Convert.ToBoolean(ConfigurationManager.AppSettings["enable-recaptcha"]);

            if (enabled)
            {
                var captchaResponse = filterContext.HttpContext.Request.Form["recaptcha"];

                if (string.IsNullOrWhiteSpace(captchaResponse))
                {
                    SetErrorResponse(filterContext);
                }
                else
                {
                    var validateResult = ValidateFromGoogle(captchaResponse);

                    if (!validateResult.Success)
                        SetErrorResponse(filterContext);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private void SetErrorResponse(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("Captcha Inválido");
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
            filterContext.Result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private ReCaptchaResponse ValidateFromGoogle(string captchaResponse)
        {
            var url = ConfigurationManager.AppSettings["url-recaptcha"];
            var key = ConfigurationManager.AppSettings["secret-key-recaptcha"];

            var client = new WebClient();

            var response = client.DownloadString($"{url}?secret={key}&response={captchaResponse}");

            var result = JsonConvert.DeserializeObject<ReCaptchaResponse>(response);

            return result;
        }
    }

    internal class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}