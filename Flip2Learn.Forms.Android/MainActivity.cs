using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Flip2Learn.Shared.Application;
using System.IO;
using System.Collections.Generic;
using XF.Material.Forms;
using System.Linq;
using Xamarin.Forms;
using Android.Content.Res;

namespace Flip2Learn.Forms.Droid
{
    public class Android_CrossApplication : CrossApplication
    {
        private Android_CrossApplication() : base()
        {

        }

        public static ICrossApplication Instance()
        {
            lock (typeof(CrossApplication))
            {
                if (instance == null)
                    instance = new Android_CrossApplication();

                return instance;
            }
        }


        public override ICrossApplication App => this;
    }



    /// <summary>
    /// 
    /// </summary>
    [Activity(Label = "Flip2Learn", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CachedImageRenderer.Init(true);

            Android_CrossApplication.Instance();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var app = new App();
            LoadApplication(app);
            Material.Init(app);

            SetAppTheme();
        }


        /// <summary>
        /// 
        /// </summary>
        void SetAppTheme()
        {
            if (Resources.Configuration.UiMode.HasFlag(UiMode.NightYes))
                App.ChangeTheme(true);
            else
                App.ChangeTheme(false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}