using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Flip2Learn.Shared.Application;
using Foundation;
using Google.MobileAds;
using UIKit;

namespace Flip2Learn.Forms.iOS
{
    public class iOS_CrossApplication : CrossApplication
    {
        public override event EventHandler AdsReady = delegate { };

        private iOS_CrossApplication()
            : base()
        {

        }

        public static ICrossApplication Instance()
        {
            lock (typeof(CrossApplication))
            {
                if (instance == null)
                    instance = new iOS_CrossApplication();

                return instance;
            }
        }

        public override ICrossApplication App => this;

        private static AdLoader loader;

        /// <summary>
        /// 
        /// </summary>
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

                    var option1 = new NativeAdImageAdLoaderOptions()
                    {
                        DisableImageLoading = true,
                        ShouldRequestMultipleImages = false
                    };

                    var option2 = new NativeAdViewAdOptions()
                    {
                        PreferredAdChoicesPosition = AdChoicesPosition.TopRightCorner
                    };

                    var option3 = new VideoOptions()
                    {
                        StartMuted = false
                    };

                    loader = new AdLoader(AD_ID, AppDelegate.Instance.Window.RootViewController, new AdLoaderAdType[] { AdLoaderAdType.UnifiedNative }, new AdLoaderOptions[] { option1, option2, option3 });
                    loader.Delegate = AppDelegate.Instance;
                }

                if (!loader.IsLoading)
                {
                    if (LoadedAd == null || force)
                    {
                        loader.LoadRequest(Request.GetDefaultRequest());
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
            LoadedAd = new iOSNativeAd(ad);
            AdsReady(this, EventArgs.Empty);
        }


        /// <summary>
        /// 
        /// </summary>
        public class iOSNativeAd : INativeAd
        {
            public string Id => null;

            public string Headline => source.Headline;

            public string Body => source.Body;

            public string AdvetiserIconUrl => source.Icon?.ImageUrl?.ToString();

            public string AdvetiserName => source.Advertiser;

            public string ImageUrl => source.Images?.FirstOrDefault()?.ImageUrl?.ToString();

            public string Button => source.CallToAction;

            public object NativeAdSource => source;

            private readonly UnifiedNativeAd source;

            public iOSNativeAd(UnifiedNativeAd source)
            {
                this.source = source;
            }
        }

    }


    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAdLoaderDelegate, IUnifiedNativeAdLoaderDelegate
    {
        public static AppDelegate Instance;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Instance = this;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            MobileAds.SharedInstance.Start((a) => { });

            iOS_CrossApplication.Instance();


            global::Xamarin.Forms.Forms.Init();
            XF.Material.iOS.Material.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        public void DidFailToReceiveAd(AdLoader adLoader, RequestError error)
        {

        }

        public void DidReceiveUnifiedNativeAd(AdLoader adLoader, UnifiedNativeAd nativeAd)
        {
            (CrossApplication.instance as iOS_CrossApplication).OnUnifiedNativeAdLoaded(nativeAd);
        }
    }
}
