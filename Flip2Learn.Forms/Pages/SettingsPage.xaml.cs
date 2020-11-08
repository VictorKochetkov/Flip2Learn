using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Flip2Learn.Forms.Views;
using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Models;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Pages
{
    public partial class SettingsPage : AppContentPage, ISearchPage
    {
        public event EventHandler<string> SearchBarTextChanged;
        public string SearchPlaceholder => "Search";

        private string SearchText = string.Empty;

        public SettingsPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (listView.ItemTemplate == null)
            {
                listView.ItemTemplate = new DataTemplate(typeof(SelectCountryCell));
                listView.ItemsSource = app.GetSelectCountryList().Where(x => x.IsMatch(SearchText));
            }

            app.AppChanged -= App_AppChanged;
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
        private void UpdateKnownCountries()
        {
            knownCountries.SetText($"✔️ {app.GetKnownCountriesCount()}");
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void OnSearchBarTextChanged(string text)
        {
            this.SearchText = text;

            var items = listView.ItemsSource;
            listView.ItemsSource = null;
            listView.ItemsSource = items;
        }
    }
}
