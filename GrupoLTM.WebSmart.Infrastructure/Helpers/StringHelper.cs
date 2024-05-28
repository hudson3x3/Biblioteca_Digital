using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;

namespace GrupoLTM.WebSmart.Infrastructure.Helpers
{
    public class StringHelper
    {
        public static string ReplaceEnconding(string strString)
        {
            strString = strString.Replace("Á", "&Aacute;");
            strString = strString.Replace("á", "&aacute;");
            strString = strString.Replace("Â", "&Acirc;");
            strString = strString.Replace("â", "&acirc;");
            strString = strString.Replace("À", "&Agrave;");
            strString = strString.Replace("à", "&agrave;");
            strString = strString.Replace("Å", "&Aring;");
            strString = strString.Replace("å", "&aring;");
            strString = strString.Replace("Ã", "&Atilde;");
            strString = strString.Replace("ã", "&atilde;");
            strString = strString.Replace("Ä", "&Auml;");
            strString = strString.Replace("ä", "&auml;");
            strString = strString.Replace("Æ", "&AElig;");
            strString = strString.Replace("æ", "&aelig;");
            strString = strString.Replace("É", "&Eacute;");
            strString = strString.Replace("é", "&eacute;");
            strString = strString.Replace("Ê", "&Ecirc;");
            strString = strString.Replace("ê", "&ecirc;");
            strString = strString.Replace("È", "&Egrave;");
            strString = strString.Replace("è", "&egrave;");
            strString = strString.Replace("Ë", "&Euml;");
            strString = strString.Replace("ë", "&euml;");
            strString = strString.Replace("Ð", "&ETH;");
            strString = strString.Replace("ð", "&eth;");
            strString = strString.Replace("Í", "&Iacute;");
            strString = strString.Replace("í", "&iacute;");
            strString = strString.Replace("Î", "&Icirc;");
            strString = strString.Replace("î", "&icirc;");
            strString = strString.Replace("Ì", "&Igrave;");
            strString = strString.Replace("ì", "&igrave;");
            strString = strString.Replace("Ï", "&Iuml;");
            strString = strString.Replace("ï", "&iuml;");
            strString = strString.Replace("Ó", "&Oacute;");
            strString = strString.Replace("ó", "&oacute;");
            strString = strString.Replace("Ô", "&Ocirc;");
            strString = strString.Replace("ô", "&ocirc;");
            strString = strString.Replace("Ò", "&Ograve;");
            strString = strString.Replace("ò", "&ograve;");
            strString = strString.Replace("Ø", "&Oslash;");
            strString = strString.Replace("ø", "&oslash;");
            strString = strString.Replace("Õ", "&Otilde;");
            strString = strString.Replace("õ", "&otilde;");
            strString = strString.Replace("Ö", "&Ouml;");
            strString = strString.Replace("ö", "&ouml;");
            strString = strString.Replace("Ú", "&Uacute;");
            strString = strString.Replace("ú", "&uacute;");
            strString = strString.Replace("Û", "&Ucirc;");
            strString = strString.Replace("û", "&ucirc;");
            strString = strString.Replace("Ù", "&Ugrave;");
            strString = strString.Replace("ù", "&ugrave;");
            strString = strString.Replace("Ü", "&Uuml;");
            strString = strString.Replace("ü", "&uuml;");
            strString = strString.Replace("Ç", "&Ccedil;");
            strString = strString.Replace("ç", "&ccedil;");
            strString = strString.Replace("Ñ", "&Ntilde;");
            strString = strString.Replace("ñ", "&ntilde;");
            return strString;
        }

        public static string GetSubstringByString(string leftDelimiter, string rightDelimiter, string value)
        {
            return value.Substring((value.IndexOf(leftDelimiter) + leftDelimiter.Length),
                (value.IndexOf(rightDelimiter) - value.IndexOf(leftDelimiter) - leftDelimiter.Length));
        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static string FormatarCnpj(string cnpj)
        {
            double cnpjVal;
            if (double.TryParse(cnpj, out cnpjVal))
            {
                return string.Format(@"{0:00\.000\.000\/0000\-00}", cnpjVal);
            }
            else
            {
                return cnpj;
            }
        }

        public static string FormatarCpf(string cpf)
        {
            double cpfVal;
            if (double.TryParse(cpf, out cpfVal))
            {
                return string.Format(@"{0:000\.000\.000\-00}", cpfVal);
            }
            else
            {
                return cpf;
            }
        }

        public static string RemoverAcentos(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
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

        public static string URLFriendly(string title)
        {
            if (title == null) return "";

            const int maxlen = 80;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}
