using GrupoLTM.WebSmart.Infrastructure.Attributes;
using GrupoLTM.WebSmart.Infrastructure.Exceptions;
using GrupoLTM.WebSmart.Infrastructure.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public static class Helper
    {
        private static Random random = new Random();

        public static T ToEnum<T>(this int number)
        {
            return (T)Enum.Parse(typeof(T), number.ToString());
        }

        public static string GetDescription(this Enum value)
        {
            if (value?.ToString() is null)
                return null;

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Property[] GetPropertiesAndFields(this Type type)
        {
            var properties = type.GetProperties().Select(x => new Property(x.Name, x.PropertyType, propertyInfo: x));

            var fields = type.GetFields().Select(x => new Property(x.Name, x.FieldType, fieldInfo: x));

            return properties.Union(fields).ToArray();
        }

        public static Property[] GetProperties<T>()
        {
            var properties = typeof(T).GetProperties().Where(x => !x.GetGetMethod().IsVirtual).Select(x => new Property(x.Name, x.PropertyType, propertyInfo: x));

            var fields = typeof(T).GetFields().Select(x => new Property(x.Name, x.FieldType, fieldInfo: x));

            return properties.Union(fields).ToArray();
        }

        public static DataColumn[] GetStructure<T>()
        {
            var properties = GetProperties<T>();
            return properties.Select(x => new DataColumn(x.Name, Nullable.GetUnderlyingType(x.Type) ?? x.Type)).ToArray();
        }

        public static DataColumn ToDataColumn(this Property property)
        {
            return new DataColumn(property.Name, Nullable.GetUnderlyingType(property.Type) ?? property.Type);
        }

        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var dict = props.ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));
            return dict;
        }

        public static IEnumerable<string> GetColumns(this DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(p => p.Field<string>("ColumnName"));
        }

        public static Dictionary<string, Property[]> GetPropsTypeField(this Type type)
        {
            var propertiesType = new List<KeyValuePair<string, Property>>();

            var allProps = type.GetPropertiesAndFields();

            foreach (var prop in allProps)
            {
                var attribute = prop.GetAttributes().Select(x => x as FieldTypeAttribute).FirstOrDefault(x => x != null);

                foreach (var key in attribute.Types)
                    propertiesType.Add(new KeyValuePair<string, Property>(key, prop));
            }

            var dic = propertiesType.GroupBy(x => x.Key).ToDictionary(x => x.Key, y => y.Select(q => q.Value).ToArray());

            return dic;
        }

        public static IEnumerable<T[]> GroupSize<T>(this IEnumerable<T> items, int size)
        {
            if (items == null || size <= 0)
                return null;

            return items.Select((item, index) => new { item, index })
                        .GroupBy(pair => pair.index / size, pair => pair.item)
                        .Select(grp => grp.ToArray());
        }

        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            var innerException = ex;

            do
            {
                yield return innerException;
                innerException = innerException.InnerException;

            } while (innerException != null);
        }

        public static string ToJson(this object obj, bool camelCase = true)
        {
            var settings = new JsonSerializerSettings();

            if (camelCase)
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static bool IsSuccessStatusCode(this IRestResponse response)
        {
            var statusCode = (int)response.StatusCode;
            return statusCode >= 200 && statusCode <= 399;
        }

        public static void CheckTemp(string path)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists)
                directory.GetFiles().ToList().ForEach(file => file.Delete());
            else
                directory.Create();
        }

        public static ProcessamentoException ToProcException(this Exception ex, string nomeArquivo = null)
        {
            return new ProcessamentoException(ex.Message, nomeArquivo, ex.GetBaseException());
        }

        public static int RandomBetween(int minValue, int maxValue)
        {
            lock (random)
            {
                return random.Next(minValue, maxValue);
            }
        }

        public static List<T> ToList<T>(this DataTable datatable)
        {
            var columnNames = datatable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

            var properties = typeof(T).GetProperties();

            var list = new List<T>();

            foreach (var row in datatable.AsEnumerable())
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var prop in properties)
                {
                    if (columnNames.Contains(prop.Name))
                        prop.SetValue(obj, row[prop.Name]);
                }

                list.Add(obj);
            }

            return list;
        }

        public static string RemoveText(this string content, params string[] texts)
        {
            if (content is null)
                return content;

            foreach (var text in texts)
                content = content.Replace(text, string.Empty);

            return content;
        }

        public static string OnlyNumbers(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return Regex.Replace(text, "[^0-9]", string.Empty);
        }
    }
}