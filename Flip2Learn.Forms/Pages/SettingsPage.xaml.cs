using System;
using System.Collections.Generic;
using System.Linq;
using Flip2Learn.Forms.Views;
using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Models;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Pages
{
    public partial class SettingsPage : AppContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            listView.ItemTemplate = new DataTemplate(typeof(SelectCountryCell));
            listView.ItemsSource = app.GetSelectCountryList();

            UpdateKnownCountries();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateKnownCountries()
        {
            knownCountries.SetText($"✔️ {app.GetKnownCountriesCount()}");
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            app.AppChanged += App_AppChanged;

            UpdateKnownCountries();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            app.AppChanged -= App_AppChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_AppChanged(object sender, Shared.Application.AppChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (e.ChangedType)
                {
                    case AppChangedType.KnownCountries:
                        UpdateKnownCountries();
                        break;
                }
            });
        }
    }
}
