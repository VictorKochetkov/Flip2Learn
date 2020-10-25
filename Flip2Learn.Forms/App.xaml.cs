using System;
using Flip2Learn.Forms.Pages;
using Flip2Learn.Forms.Themes;
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


        /// <summary>
        /// 
        /// </summary>
        public static bool IsDark { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dark"></param>
        public static void ChangeTheme(bool dark)
        {
            if (dark == IsDark)
                return;

            IsDark = dark;

            if (dark)
                Current.Resources = new DarkTheme();
            else
                Current.Resources = new LightTheme();
        }
    }
}
