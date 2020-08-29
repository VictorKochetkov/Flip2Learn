using Newtonsoft.Json;
using System.Collections.Generic;

namespace Flip2Learn.Shared.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizedString
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("en")]
        public string en { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ru")]
        public string ru { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("ru_genetive")]
        public string ru_genetive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public LocalizedString() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ru"></param>
        /// <param name="en"></param>
        public LocalizedString(string ru, string en)
        {
            this.ru = ru;
            this.en = en;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="genetive"></param>
        /// <returns></returns>
        public string GetLocalized(string locale, bool genetive = false)
        {
            if (locale.ToLower().Contains("ru"))
                return genetive ? this.ru_genetive : this.ru;
            else
                return this.en;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{en};{ru}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is LocalizedString @string &&
                   en == @string.en &&
                   ru == @string.ru &&
                   ru_genetive == @string.ru_genetive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 950943196;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(en);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ru);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ru_genetive);
            return hashCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool operator ==(LocalizedString str1, LocalizedString str2)
        {
            return str1.en == str2.en && str1.ru == str2.ru;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool operator !=(LocalizedString str1, LocalizedString str2)
        {
            return str1.en != str2.en || str1.ru != str2.ru;
        }
    }
}
