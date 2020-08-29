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



    [Activity(Label = "Flip2Learn", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
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
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}