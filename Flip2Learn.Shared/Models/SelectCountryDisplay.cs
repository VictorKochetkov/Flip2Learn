using System;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Database;
using Flip2Learn.Shared.Helpers;

namespace Flip2Learn.Shared.Models
{
    public interface ISelectCountryDisplay : IRow
    {
        bool IsKnown { get; }
        string Flag { get; }
    }


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
        public string Flag => QuestionDisplay.GetFlag(Source);

        /// <summary>
        /// 
        /// </summary>
        public bool IsKnown => snapshot.IsMarkedAsKnown;


        private CountrySnapshot snapshot;

        public SelectCountryDisplay(Country source) : base(source)
        {
            this.snapshot = app.FindSnapshotOrCreate(source.NameAsId());
        }


    }
}
