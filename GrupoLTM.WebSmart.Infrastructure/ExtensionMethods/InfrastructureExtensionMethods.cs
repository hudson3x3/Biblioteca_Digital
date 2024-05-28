using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace GrupoLTM.WebSmart.Infrastructure.ExtensionMethods
{
    public static class InfrastructureExtensionMethods
    {
        public static string RemoveFormatacaoCPF(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return string.Empty;
            else
                return cpf.Replace(".", "").Replace("-", "");
        }

        public static string RemoverAcentos(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;
            else
            {
                byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
        }

        public static string RemoverMascara(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            texto = texto.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char c in texto.ToCharArray())
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber)
                    sb.Append(c);
            return sb.ToString();
        }

        public static bool IsNull(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static Int16 ToInt16(this string value)
        {
            Int16 result = 0;

            if (!String.IsNullOrEmpty(value))
                Int16.TryParse(value, out result);

            return result;
        }

        public static Int32 ToInt32(this string value)
        {
            Int32 result = 0;

            if (!String.IsNullOrEmpty(value))
                Int32.TryParse(value, out result);

            return result;
        }

        public static Int64 ToInt64(this string value)
        {
            Int64 result = 0;

            if (!String.IsNullOrEmpty(value))
                Int64.TryParse(value, out result);

            return result;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return (list == null || (list != null && list.Count <= 0));
        }

        public static string ToSafeString(this object obj)
        {
            return obj != null ? obj.ToString() : String.Empty;
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(Regex.Replace(str,
                                @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string RemoverSufixo(this string str)
        {
            string splittedString = Regex.Replace(Regex.Replace(str,
                                @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            return splittedString.Substring(0, splittedString.LastIndexOf(' ')).Replace(" ", "");

        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection) action(item);
            return collection;
        }

        public static List<T> AddDefaultItem<T>(this List<T> list, string dataValueField, string dataTextField, string dataValueValue = "0", string dataTextValue = "Selecione")
        {
            try
            {
                var instance = Activator.CreateInstance(typeof(T));
                PropertyInfo propertyInfo;
                if (instance.GetType().GetProperties().Select(x => x.Name).Contains(dataTextField))
                {
                    propertyInfo = instance.GetType().GetProperty(dataTextField);
                    propertyInfo.SetValue(instance, Convert.ChangeType(dataTextValue, propertyInfo.PropertyType),
                                          null);
                }

                if (instance.GetType().GetProperties().Select(x => x.Name).Contains(dataValueField))
                {
                    propertyInfo = instance.GetType().GetProperty(dataValueField);
                    propertyInfo.SetValue(instance, Convert.ChangeType(dataValueValue, propertyInfo.PropertyType),
                                          null);
                }
                list.Insert(0, (T)instance);
            }
            catch
            {
                return list;
            }
            return list;
        }

        public static bool CompareDateOnly(this DateTime date, DateTime other)
        {
            return (date.Year == other.Year && date.Month == other.Month && date.Day == other.Day);
        }

        public static bool InHour(this DateTime date, DateTime compareDate)
        {
            return date.CompareDateOnly(compareDate) && date.Hour == compareDate.Hour;
        }

        public static bool InHour(this DateTime? date, DateTime compareDate)
        {
            if (!date.HasValue)
                return false;

            return date.Value.CompareDateOnly(compareDate) && date.Value.Hour == compareDate.Hour;
        }

        public static string ToShortDateAndTimeString(this DateTime dateTime)
        {
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }

        public static ICollection<T> SetInativos<T>(this ICollection<T> list)
        {
            foreach (var item in list)
            {
                PropertyInfo propertyAtivo = item.GetType().GetProperty("Ativo");
                if (propertyAtivo != null)
                    propertyAtivo.SetValue(item, Convert.ChangeType(false, propertyAtivo.PropertyType), null);
            }
            return list;
        }

        public static object GetPropertyValue(this object srcobj, string propertyName)
        {
            if (srcobj == null)
                return null;

            object obj = srcobj;

            // Split property name to parts (propertyName could be hierarchical, like obj.subobj.subobj.property
            string[] propertyNameParts = propertyName.Split('.');

            foreach (string propertyNamePart in propertyNameParts)
            {
                if (obj == null) return null;

                // propertyNamePart could contain reference to specific 
                // element (by index) inside a collection
                if (!propertyNamePart.Contains("["))
                {
                    PropertyInfo pi = obj.GetType().GetProperty(propertyNamePart);
                    if (pi == null) return null;
                    obj = pi.GetValue(obj, null);
                }
                else
                {   // propertyNamePart is areference to specific element 
                    // (by index) inside a collection
                    // like AggregatedCollection[123]
                    //   get collection name and element index
                    int indexStart = propertyNamePart.IndexOf("[") + 1;
                    string collectionPropertyName = propertyNamePart.Substring(0, indexStart - 1);
                    int collectionElementIndex = Int32.Parse(propertyNamePart.Substring(indexStart, propertyNamePart.Length - indexStart - 1));
                    //   get collection object
                    PropertyInfo pi = obj.GetType().GetProperty(collectionPropertyName);
                    if (pi == null) return null;
                    object unknownCollection = pi.GetValue(obj, null);
                    //   try to process the collection as array
                    if (unknownCollection.GetType().IsArray)
                    {
                        object[] collectionAsArray = unknownCollection as Array[];
                        obj = collectionAsArray[collectionElementIndex];
                    }
                    else
                    {
                        //   try to process the collection as IList
                        System.Collections.IList collectionAsList = unknownCollection as System.Collections.IList;
                        if (collectionAsList != null)
                        {
                            obj = collectionAsList[collectionElementIndex];
                        }
                        else
                        {
                            // ??? Unsupported collection type
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Verifica se o objeto é nulo
        /// </summary>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Verifica se a string é nula ou vazia
        /// </summary>
        public static bool IsNullOrEmpty(this string valor)
        {
            return String.IsNullOrEmpty(valor);
        }

        /// <summary>
        /// Retorna o inteiro informado ou NULL se o valor for 0
        /// </summary>
        /// <returns></returns>
        public static int? NullIfZero(this int valor)
        {
            return valor > 0 ? valor : (int?)null;
        }

        /// <summary>
        /// Retorna o inteiro informado ou NULL se o valor for 0
        /// </summary>
        /// <returns></returns>
        public static int? NullIfZero(this int? valor)
        {
            return valor.HasValue && valor.Value > 0 ? valor : null;
        }

        /// <summary>
        /// Método que converte um texto no formato de Propriedade (Sem acentos, primeira letra maiúscula...)
        /// </summary>
        /// <param name="texto">Texto a ser formatado</param>
        /// <returns>Testo formatado como nome de propriedade</returns>
        public static string ToPropertyFormat(this string texto)
        {
            string retorno = string.Empty;

            // Caso texto tenha vindo em palavras separadas por espaço
            if (texto.IndexOf(" ") != -1)
            {
                // Deixa todas as primeiras letras em maíusculo
                CultureInfo currentInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = currentInfo.TextInfo;

                retorno = textInfo.ToTitleCase(texto);
            }
            else
            {
                // deixa apenas primeira letra mauiscula
                var first = texto.ToList<char>().First();

                retorno = string.Format("{0}{1}", first.ToString().ToUpper(), texto.Substring(1, texto.Length - 1));
            }

            // Remove acentos
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(retorno);
            retorno = System.Text.Encoding.UTF8.GetString(bytes);

            // Remove espaços e caracteres especiais
            retorno = retorno.Replace(" ", "").Replace("-", "");

            return retorno;
        }

        /// <summary>
        /// Método que transforma o texto para comparação segura (deixa maiusculo e sem acentos)
        /// </summary>
        /// <param name="texto">Texto a ser transformado</param>
        /// <returns>Texto transforamdo para comparação segura</returns>
        public static string ToComparationFormat(this string texto)
        {
            string retorno = texto.ToUpper();

            // Remove acentos
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(retorno);
            retorno = System.Text.Encoding.UTF8.GetString(bytes);

            return retorno;
        }

        private static Tuple<string, object[]> PrepareArguments(string storedProcedure, object parameters)
        {
            var parameterNames = new List<string>();
            var parameterParameters = new List<object>();

            if (parameters != null)
            {
                foreach (PropertyInfo propertyInfo in parameters.GetType().GetProperties())
                {
                    string name = "@" + propertyInfo.Name;
                    object value = propertyInfo.GetValue(parameters, null);

                    parameterNames.Add(name);
                    parameterParameters.Add(new SqlParameter(name, value ?? DBNull.Value));
                }
            }

            if (parameterNames.Count > 0)
                storedProcedure += " " + string.Join(", ", parameterNames);

            return new Tuple<string, object[]>(storedProcedure, parameterParameters.ToArray());
        }

        public static string GetEnumDescription<TEnum>(this TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static string ToReplace(this string value, string oldValue, string newValue)
        {
            return !String.IsNullOrEmpty(value) ? value.Replace(oldValue, newValue) : value;
        }

        public static string Pluralize(double quantidade, string singular, string plural)
        {
            if (quantidade > -2 && quantidade < 2)
                return singular;

            return plural;
        }

        public static double ToPontos(this string valor)
        {
            return !String.IsNullOrEmpty(valor) ? Convert.ToDouble(valor.Replace(".", ",")) : 0;
        }

        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}
