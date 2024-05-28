using System;
using System.Web;

namespace GrupoLTM.WebSmart.Infrastructure.Cache
{
    public static class WebCache
    {
        public static T GetCache<T>(string name, long? id = null)
        {
            try
            {
                name = id != null ? string.Format("{0}_{1}", name, id) : name;

                return (T)HttpContext.Current.Cache[name];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetCache(string key, string value, int expireSeconds = 5)
        {
            try
            {
                HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddSeconds(expireSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetCache<T>(string name, T value, long? id = null)
        {
            try
            {
                name = id != null ? string.Format("{0}_{1}", name, id) : name;

                HttpContext.Current.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(15), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
