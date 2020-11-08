using System;
using Flip2Learn.Shared.Application;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AppContentPage : ContentPage
    {
        protected ICrossApplication app => CrossApplication.instance;

        public void OnFirstAppearing()
        {
            OnAppearing();
        }

    }
}
