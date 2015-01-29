namespace FMS.Web.Helper.Extensions
{
    using Microsoft.Security.Application;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Security;
    using System.Linq;

    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return string.Format(CultureInfo.CurrentCulture, target, args);
        }

        [DebuggerStepThrough]
        public static string Hash(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        [DebuggerStepThrough]
        public static string ToMdfive(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");
            return FormsAuthentication.HashPasswordForStoringInConfigFile(target, FormsAuthPasswordFormat.MD5.ToString());

        }

        [DebuggerStepThrough]
        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            Check.Argument.IsNotEmpty(target, "target");
            Check.Argument.IsNotNegativeOrZero(index, "index");

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                return StripHTMLExpression.Replace(target, string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }

        [DebuggerStepThrough]
        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        //[DebuggerStepThrough]
        //public static string JavaScriptEncode(this string target)
        //{
        //    return AntiXss.JavaScriptEncode(target);
        //}

        [DebuggerStepThrough]
        public static string ToKiloByte(this int target)
        {
            string fileSize = "{0} B".Trim().FormatWith(target);
            int kiloBytes = (int)Math.Ceiling(target / 1024M);
            if (target > 800)
            {
                fileSize = "{0} KB".Trim().FormatWith(kiloBytes);
            }

            return fileSize;
        }

        [DebuggerStepThrough]
        public static string GetDescription(this Enum currentEnum)
        {
            string description = String.Empty;

            FieldInfo fieldInfo = currentEnum.GetType().GetField(currentEnum.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
            if (attribute != null)
                description = attribute.Description;
            else
                description = currentEnum.ToString();

            return description;
        }

        public static string WithOwnership(this string target)
        {
            if (target[target.Length - 1] == 's')
            {
                return target + "'";
            }
            return target + "'s"; ;
        }

        public static string WithLineBreaks(this string target)
        {
            if (!String.IsNullOrEmpty(target))
            {
                return target.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            }

            return target;
        }

        public static bool EqualsIgnoreCase(this string target, string dataToCompare)
        {
            if (target == null && dataToCompare == null) return true;

            if (target == null || dataToCompare == null) return false;

            return target.Equals(dataToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static string GetOriginalSize(this string target)
        {
            string result = target;
            string filename = Path.GetFileName(target);
            string[] parts = filename.Split('.');
            if (parts.Length > 3)
            {
                result = target.Replace("{0}.{1}.".FormatWith(parts[0], parts[1]), "");
            }
            return result;
        }

        public static string Sanitize(this string input)
        {

            return (String.IsNullOrWhiteSpace(input)) ? string.Empty : GetSanitize(input.Trim());
        }

        private static string GetSanitize(string p)
        {

            string result = string.Empty;
            try
            {
                p.Replace("&quot;", "'");
                p.Replace("&amp;", "&");
                p.Replace("&nbsp;", " ");
                var html = p.HtmlDecode();
                result = Sanitizer.GetSafeHtmlFragment(html);
                var urlEncode = result.StripHtml().UrlDecode();
                result = Sanitizer.GetSafeHtmlFragment(urlEncode);
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

       

        public static string ToUrlFriendly(this string target)
        {

            var input = (target ?? string.Empty).Trim().ToLower();

            var sb = new StringBuilder();



            foreach (var ch in input)
            {
                if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'z'))
                {
                    sb.Append(ch);
                }
                else switch (ch)
                    {
                        case ' ':
                        case '-':
                            sb.Append('-');
                            break;
                        case '&':
                            sb.Append("and");
                            break;
                        case '?':
                            break;
                        default:
                            if (IllegalUrlCharacters.Any(x => ch.Equals(x)))
                            {
                                sb.Append('-');
                            }
                            break;
                    }
            }

            var result = sb.ToString();

            while (result.Contains("--"))
            {
                result = result.Replace("--", "-");
            }

            if (result.EndsWith("-") && result.Length > 1)
            {
                return result.Substring(0, result.LastIndexOf('-'));
            }

            return result;
        }

        public static string ToCommaSeperatedList(this int[] array)
        {

            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < array.Length; x++)
            {
                sb.Append(string.Format("{0}", array[x]));
                if (x < (array.Length - 1))
                    sb.Append(",");
            }

            return sb.ToString();

        }

        public static string NumberToText(this int target)
        {
            if (target < 0)
                return "Minus " + NumberToText(-target);
            if (target == 0)
                return "";
            if (target <= 19)
                return new string[] {"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", 
                    "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", 
                    "Seventeen", "Eighteen", "Nineteen"}[target - 1] + " ";
            if (target <= 99)
                return new string[] {"Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", 
                    "Eighty", "Ninety"}[target / 10 - 2] + " " + NumberToText(target % 10);
            if (target <= 199)
                return "One Hundred " + NumberToText(target % 100);
            if (target <= 999)
                return NumberToText(target / 100) + "Hundreds " + NumberToText(target % 100);
            if (target <= 1999)
                return "One Thousand " + NumberToText(target % 1000);
            if (target <= 999999)
                return NumberToText(target / 1000) + "Thousands " + NumberToText(target % 1000);
            if (target <= 1999999)
                return "One Million " + NumberToText(target % 1000000);
            if (target <= 999999999)
                return NumberToText(target / 1000000) + "Millions " + NumberToText(target % 1000000);
            if (target <= 1999999999)
                return "One Billion " + NumberToText(target % 1000000000);

            return NumberToText(target / 1000000000) + "Billions " + NumberToText(target % 1000000000);
        }

        public static string FirstWordLower(this string target)
        {
            if (!target.IsNullOrEmpty())
            {
                var words = target.Split(' ');
                if (words.Length >= 0)
                {
                    return words[0].ToLower();
                }

                return target.ToLower();

            }

            return target;

        }

        public static string StripAllSpaces(this string target)
        {
            return target.Replace(" ", "");
        }

        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        public static string ToImageUrl(this string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    return target.Replace("http://", "https://");
                }
                else
                {
                    return target;
                }
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Add superscript HTML markup to some symbols
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string CustomFormat(this string target)
        {
            return !string.IsNullOrEmpty(target) ? target.Replace("®", "<sup class=x-small>®</sup>") : target;
        }
    }


}