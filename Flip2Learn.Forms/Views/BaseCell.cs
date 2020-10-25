using System;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Views
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseCell<T> : ViewCell where T : class
    {
        protected T Model => (T)BindingContext;

        public BaseCell() : base()
        {
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateViews();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateViews();
        }



        protected abstract void UpdateViews();
    }
}
