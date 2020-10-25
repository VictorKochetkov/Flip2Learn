using System;
using Flip2Learn.Shared.Models;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Views
{
    public abstract class BaseRowCell<T> : BaseCell<T> where T : class, IRow
    {
        protected abstract Label Title { get; }
        protected abstract Label Subtitle { get; }

        protected override void UpdateViews()
        {
            Title?.SetText(Model?.Title);
            Subtitle?.SetText(Model?.Subtitle);
        }
    }
}
