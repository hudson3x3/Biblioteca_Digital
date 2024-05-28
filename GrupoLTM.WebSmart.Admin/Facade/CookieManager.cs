using GrupoLTM.WebSmart.Infrastructure.Cripto;
using GrupoLTM.WebSmart.Services.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace GrupoLTM.WebSmart.Admin.Facade
{
    public class CookieManager : IStateManager
    {
        /// <summary>
        /// Only implement string type of input and return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (IsSet(key))
            {
                return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[key].Value, typeof(T), CultureInfo.InvariantCulture);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("Chave {0} não encontrada.", key));
            }
        }

        public void Set(string key, string data)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = data;
            //Default value
            cookie.Expires = DateTime.Now.AddMinutes(20);
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime">Minutes</param>
        public void Set(string key, string data, int cacheTime)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = data;
            cookie.Expires = DateTime.Now.AddMinutes(cacheTime);

            if (IsSet(key))
                Remove(key);

            HttpContext.Current.Response.SetCookie(cookie);
        }

        public bool IsSet(string key)
        {
            return HttpContext.Current.Request.Cookies[key] != null;
        }

        public void Remove(string key)
        {
            if (IsSet(key))
            {
                HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
            else
            {
                //throw new KeyNotFoundException(string.Format("Chave {0} não encontrada.", key));
            }
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            string[] myCookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// Object can't be implemented with cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set(string key, object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// /// Object can't be implemented with cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object data, int cacheTime)
        {
            throw new NotImplementedException();
        }

        public static Models.LoginModel CurrentParticipanteModel
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Request.Cookies.Get("admUserKey") == null)
                        return null;

                    var jsonCookie = AES.Decrypt256(Convert.FromBase64String(HttpContext.Current.Request.Cookies.Get("admUserKey").Value));
                    var jsonParsed = JsonConvert.DeserializeObject<Models.LoginModel>(jsonCookie);

                    return jsonParsed;
                }
                catch (Exception ex)
                {
                    var log = new LogControllerModel
                    {
                        Error = ex.Message,
                        Date = DateTime.Now,
                        Class = "CookieManager",
                        Method = "CurrentParticipanteModel",
                        StackTrace = ex.StackTrace,
                        Source = ex.ToString()
                    };

                    //TODO: Update DataDog
                    //GrayLogService.LogError(log);

                    return null;
                }
            }
        }

    }
}
