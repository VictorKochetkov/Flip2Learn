﻿using System;

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
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using static Android.Gms.Ads.Formats.UnifiedNativeAd;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Flip2Learn.Forms.Droid
{
    public class __AdListener : AdListener
    {
        public override void OnAdClicked()
        {
            base.OnAdClicked();
        }
    }


    public class Android_CrossApplication : CrossApplication
    {
        public override event EventHandler AdsReady = delegate { };

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

        private static AdLoader loader;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override void LoadAd(bool force = false)
        {
            try
            {
                if (loader == null)
                {
#if RELEASE
                    const string AD_ID = "ca-app-pub-7094228861281666/7704582705";
#else
                    const string AD_ID = "ca-app-pub-3940256099942544/2247696110";
#endif

                    loader = new AdLoader.Builder(MainActivity.Instance, AD_ID)
                        .ForUnifiedNativeAd(MainActivity.Instance)
                        .WithAdListener(new __AdListener())
                        .WithNativeAdOptions(new NativeAdOptions.Builder().Build())
                        .Build();
                }

                if (!loader.IsLoading)
                {
                    if (LoadedAd == null || force)
                    {
                        (LoadedAd?.NativeAdSource as UnifiedNativeAd)?.Destroy();
                        loader.LoadAd(new AdRequest.Builder().Build());
                    }
                }

            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ad"></param>
        public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
        {
            LoadedAd = new AndroidNativeAd(ad);
            AdsReady(this, EventArgs.Empty);
        }



        /// <summary>
        /// 
        /// </summary>
        public class AndroidNativeAd : INativeAd
        {
            public string Id => null;
            public string Headline => source.Headline;
            public string Body => source.Body;
            public string AdvetiserIconUrl => source.Icon?.Uri?.ToString();
            public string AdvetiserName => source.Advertiser;
            public string ImageUrl => source.Images?.FirstOrDefault()?.Uri?.ToString();
            public string Button => source.CallToAction;
            public object NativeAdSource => source;


            private readonly UnifiedNativeAd source;

            public AndroidNativeAd(UnifiedNativeAd source)
            {
                this.source = source;
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    [Activity(Label = "Flip2Learn", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnUnifiedNativeAdLoadedListener
    {
        /// <summary>
        /// 
        /// </summary>
        public static MainActivity Instance { get; private set; }
        public static App FormsApp { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MobileAds.Initialize(this);
            CachedImageRenderer.Init(true);

            Android_CrossApplication.Instance();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);

            FormsApp = new App();
            LoadApplication(FormsApp);
            

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ad"></param>
        public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
        {
            (Android_CrossApplication.Instance() as Android_CrossApplication).OnUnifiedNativeAdLoaded(ad);
        }
    }
}