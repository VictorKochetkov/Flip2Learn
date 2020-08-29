using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Database;
using Flip2Learn.Shared.Helpers;
using Newtonsoft.Json;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flip2Learn.Shared.Core
{
    public enum ConstitutionalForm
    {
        Republic,
        Constitutional_Monarchy,
        Absolute_Monarchy,
        None
    }

    public enum Continent
    {
        Antarctica,
        Africa,
        Asia,
        Europe,
        NorthAmerica,
        SouthAmerica,
        Oceania
    }

    public enum Recognition
    {
        Full, Partial, No
    }


    [DebuggerDisplay("{ToString()}")]
    public partial class Country
    {
        [JsonProperty("name")]
        public LocalizedString Name { get; set; }

        [JsonProperty("capital")]
        public LocalizedString Capital { get; set; }

        [JsonProperty("continent")]
        public Continent Continent { get; set; }

        [JsonProperty("area")]
        public double Area { get; set; }

        [JsonProperty("polity")]
        public ConstitutionalForm ConstitutionalForm { get; set; }

        [JsonProperty("complexity")]
        public int Complexity { get; set; }

        [JsonProperty("iso")]
        public string Iso { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Is dependent (colony, overseas territory)
        /// </summary>
        [JsonProperty("is_dependent")]
        public bool IsDependent { get; set; }

        /// <summary>
        /// Eng name of parent country if <see cref="IsDependent"/>=true
        /// </summary>
        [JsonProperty("parent")]
        public string ParentCountry { get; set; }

        /// <summary>
        /// Recognition of country
        /// </summary>
        [JsonProperty("recognition")]
        public Recognition Recognition { get; set; }


        /// <summary>
        /// Is country's name and it's capital is the same (Monaco=Monaco, Vatican=Vatican-City, Braliz=Brasilia, etc)
        /// </summary>
        /// <returns></returns>
        public bool IsSameCapitalAndName()
        {
            string name = Name.GetLocalized().ToLower();
            string capital = Capital.GetLocalized().ToLower();

            if (name == capital)
                return true;

            if (name.Contains(capital) || capital.Contains(name))
                return true;

            if (Name.en == "Brazil")
                return true;

            return false;

        }


        /// <summary>
        /// Get knowledge of country (returns exist entry or creates new)
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="countryName"></param>
        /// <returns></returns>
        //public CountryKnowledge GetCountryKnowledge(IGameDataRepository repository, Realm realm, string countryName) => repository.GetCountryKnowledge(realm, countryName);

        public override string ToString()
        {
            return $"{Name.en}";
        }

        public string NameAsId() => NameToId(Name.en);

        public static string NameToId(string name)
        {
            var r = new Regex("/^[a-zA-Z]+$", RegexOptions.Compiled);
            return r
                .Replace(name.ToLower()
                    .Replace(" ", "")
                    .Replace("-", "")
                    .Replace(" ", ""),
                    "");
        }

        public string GetWikipediaUrl()
        {
            string locale = CrossApplication.instance.Environment.Locale.StartsWith("ru") ? "ru" : "en";
            return $"https://{locale}.wikipedia.org/wiki/{Name.GetLocalized()}";
        }
    }
}
