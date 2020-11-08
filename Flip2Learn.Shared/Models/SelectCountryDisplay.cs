using System;
using System.Linq;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Database;
using Flip2Learn.Shared.Helpers;

namespace Flip2Learn.Shared.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectCountryDisplay : IRow
    {
        /// <summary>
        /// 
        /// </summary>
        string ParentCountry { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsKnown { get; }

        /// <summary>
        /// 
        /// </summary>
        string Flag { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool IsMatch(string text);
    }


    /// <summary>
    /// 
    /// </summary>
    public class SelectCountryDisplay : BaseDisplay<Country>, ISelectCountryDisplay
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id => Source.NameAsId();

        /// <summary>
        /// 
        /// </summary>
        public string Title => Source.Name.GetLocalized();

        /// <summary>
        /// 
        /// </summary>
        public string Subtitle => Source.Capital.GetLocalized();


        /// <summary>
        /// 
        /// </summary>
        public string ParentCountry
        {
            get
            {
                if (Source.IsDependent)
                {
                    string name = app.GetAllCountries().FirstOrDefault(x => x.Name.en == Source.ParentCountry).Name.GetLocalized();
                    return $"• {name}";

                }

                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Flag => QuestionDisplay.GetFlag(Source);

        /// <summary>
        /// 
        /// </summary>
        public bool IsKnown => snapshot.IsMarkedAsKnown;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool IsMatch(string text) => Source.Name.GetLocalized().ToLower().Contains(text) || Source.Capital.GetLocalized().ToLower().Contains(text);


        private CountrySnapshot snapshot;

        public SelectCountryDisplay(Country source) : base(source)
        {
            this.snapshot = app.FindSnapshotOrCreate(source.NameAsId());
        }


    }
}
