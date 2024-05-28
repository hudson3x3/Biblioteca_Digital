using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrupoLTM.WebSmart.Infrastructure
{
    public static class Extensions
    {
        #region Extension for System.Exception
        public static Dictionary<string, StringBuilder> ToDictionary(this Exception obj)
        {
            Dictionary<string, StringBuilder> ret = new Dictionary<string, StringBuilder>();
            ret.Add("Message", new StringBuilder());
            ret.Add("StackTrace", new StringBuilder());
            ret.Add("Source", new StringBuilder());
            ret.Add("HResult", new StringBuilder());

            var aux = obj;
            int count = 0;

            while (aux != null)
            {
                var SistemaException = new SistemaException(aux.Message, aux);

                ret["Message"].Append(string.Format("Erro: {0}:{1}", count.ToString(), SistemaException.Message));
                ret["StackTrace"].Append(string.Format("Erro: {0}:{1}", count.ToString(), SistemaException.StackTrace));
                ret["Source"].Append(string.Format("Erro: {0}:{1}", count.ToString(), SistemaException.Source));
                ret["HResult"].Append(string.Format("Erro: {0}:{1}", count.ToString(), SistemaException.Code));

                aux = aux.InnerException;
                count++;
            }
            return ret;
        }

        #endregion Extension for System.Exception


        public static string Serialize(this object obj, int maxdepth = 20)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, MaxDepth = maxdepth, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include });

        }
    }
}
