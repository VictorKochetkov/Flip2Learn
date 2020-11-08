using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;

namespace Flip2Learn.Shared.Resources
{
    /// <summary>
    /// https://mindofai.github.io/Implementing-Localization-..
    /// </summary>
    public static class Translator
    {
        const string ResourceId = "Flip2Learn.Shared.Resources.AppResources";
        private static ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(Translator).GetTypeInfo().Assembly);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentCulture) ?? key;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Translate(this string text, params string[] format)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            try
            {
                string localized = Regex.Unescape(regex.Replace(text, (Match match) => GetString(match.Groups["code"].Value)));

                if (localized == text)
                    localized = GetString(localized);

                if (format?.Any() == true)
                    localized = string.Format(localized, format);

                return localized;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return text;
            }
        }


        /// <summary>
        /// The regex.
        /// </summary>
        static readonly Regex regex = new Regex(@"\$=(?<code>[\w \.\s]+)\$\$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

}