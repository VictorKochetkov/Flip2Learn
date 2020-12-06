using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Flip2Learn.Forms.Droid.Effects;
using Flip2Learn.Forms.Effects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Xamarin.Forms.View;

[assembly: ResolutionGroupName("Flip2Learn")]
[assembly: ExportEffect(typeof(Android_RotateEffect), nameof(RotateEffect))]
namespace Flip2Learn.Forms.Droid.Effects
{
    public class Android_RotateEffect : PlatformEffect
    {
        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == "Renderer")
            {
                ///https://www.thedroidsonroids.com/blog/how-to-add-card-flip-animation-to-your-android-app
                (Control ?? Container)?.SetCameraDistance(MainApplication.Context.Resources.DisplayMetrics.Density * 2000);
            }
        }

        protected override void OnAttached()
        {
        }

        protected override void OnDetached()
        {
        }

    }
}