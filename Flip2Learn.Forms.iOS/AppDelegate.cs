using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flip2Learn.Shared.Application;
using Foundation;
using UIKit;

namespace Flip2Learn.Forms.iOS
{
    public class iOS_CrossApplication : CrossApplication
    {
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
    }


    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;


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
    }
}
