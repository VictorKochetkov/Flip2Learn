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
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using static Android.Gms.Ads.Formats.UnifiedNativeAd;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.Content;
using Flip2Learn.Forms.Pages;
using INavigation = Flip2Learn.Shared.Application.INavigation;

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

        private Android_CrossApplication(INavigation navigation) : base(navigation)
        {

        }

        public static ICrossApplication Instance(INavigation navigation)
        {
            lock (typeof(CrossApplication))
            {
                if (instance == null)
                    instance = new Android_CrossApplication(navigation);

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
                        .WithNativeAdOptions(new NativeAdOptions.Builder()
                            .SetMediaAspectRatio(NativeAdOptions.NativeMediaAspectRatioLandscape)
                            .SetReturnUrlsForImageAssets(false)
                            .SetAdChoicesPlacement(NativeAdOptions.AdchoicesTopRight)
                            .SetVideoOptions(new VideoOptions.Builder()
                                .SetStartMuted(false)
                                .Build())
                            .Build())
                        .Build();

                }

                if (!loader.IsLoading)
                {
                    if (LoadedAd == null || force)
                    {
                        (LoadedAd?.NativeAdSource as UnifiedNativeAd)?.Destroy();
                        loader.LoadAd(new AdRequest.Builder()
                            .AddTestDevice("65CED65A56972B8941F0BC2A28EB6207")///Nokia 6.1
                            .Build());
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
        internal void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
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
            public Shared.Application.IMediaContent MediaContent => mediaContent;
            public object NativeAdSource => source;


            private readonly UnifiedNativeAd source;
            private readonly AndroidMediaContent mediaContent;

            public AndroidNativeAd(UnifiedNativeAd source)
            {
                this.source = source;
                this.mediaContent = new AndroidMediaContent(source.MediaContent);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public class AndroidMediaContent : Shared.Application.IMediaContent
        {
            public object NativeContentSource => source;

            public Android.Gms.Ads.IMediaContent source;
            public AndroidMediaContent(Android.Gms.Ads.IMediaContent source)
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

            Android_CrossApplication.Instance(new FormsNavigation());

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
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            Plugin.InAppBilling.InAppBillingImplementation.HandleActivityResult(requestCode, resultCode, data);
            base.OnActivityResult(requestCode, resultCode, data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ad"></param>
        public void OnUnifiedNativeAdLoaded(UnifiedNativeAd ad)
        {
            (Android_CrossApplication.instance as Android_CrossApplication).OnUnifiedNativeAdLoaded(ad);
        }
    }
}