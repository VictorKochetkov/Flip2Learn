using System;
using Flip2Learn.Forms.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new GamePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
