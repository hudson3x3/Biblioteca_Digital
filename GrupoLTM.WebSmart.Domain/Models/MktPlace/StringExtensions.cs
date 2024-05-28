using System;

namespace GrupoLTM.WebSmart.Domain.Models.MktPlace
{
    public static class StringExtensions
    {
        public static string Truncate(this string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }

        public static string TruncateWithSuffix(this string source, int length, string suffix = "...")
        {
            var result = Truncate(source, length);
            if (result.Length == length)
                result += suffix;
            return result;
        }

        public static string BuildsFriendlyUrl(string source)
        {
            source = source.ToLower();
            source = source.Replace(" ", "-");
            source = source.Replace("---", "-");
            source = source.Replace("--", "-");
            source = source.Replace("à", "a");
            source = source.Replace("á", "a");
            source = source.Replace("â", "a");
            source = source.Replace("ã", "a");
            source = source.Replace("ä", "a");
            source = source.Replace("å", "a");
            source = source.Replace("ç", "c");
            source = source.Replace("é", "e");
            source = source.Replace("è", "e");
            source = source.Replace("ê", "e");
            source = source.Replace("ë", "e");
            source = source.Replace("ì", "i");
            source = source.Replace("í", "i");
            source = source.Replace("î", "i");
            source = source.Replace("ï", "i");
            source = source.Replace("ñ", "n");
            source = source.Replace("ò", "o");
            source = source.Replace("ó", "o");
            source = source.Replace("ô", "o");
            source = source.Replace("ö", "o");
            source = source.Replace("õ", "o");
            source = source.Replace("ù", "u");
            source = source.Replace("ú", "u");
            source = source.Replace("û", "u");
            source = source.Replace("ü", "u");
            source = source.Replace("ý", "y");
            source = source.Replace("ÿ", "y");
            source = source.Replace(".", "");
            source = source.Replace(",", "");
            source = source.Replace(":", "");
            source = source.Replace(";", "");
            source = source.Replace("/", "");
            source = source.Replace(@"\", "");
            source = source.Replace("|", "");
            source = source.Replace("[", "");
            source = source.Replace("]", "");
            source = source.Replace("{", "");
            source = source.Replace("}", "");
            source = source.Replace("'", "");
            source = source.Replace("¿", "");
            source = source.Replace("!", "");
            source = source.Replace("@", "");
            source = source.Replace("#", "");
            source = source.Replace("$", "");
            source = source.Replace("%", "");
            source = source.Replace("¨", "");
            source = source.Replace("&", "e");
            source = source.Replace("*", "");
            source = source.Replace("(", "");
            source = source.Replace(")", "");
            source = source.Replace("_", "");
            source = source.Replace("+", "");
            source = source.Replace("=", "");
            //source = source.Replace("", "");

            return source.ToLower();
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value, T defaultValue)
            where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            T result;

            return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
        }
    }
}
