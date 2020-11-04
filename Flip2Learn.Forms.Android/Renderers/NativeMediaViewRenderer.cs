using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.Gms.Ads.Formats;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Flip2Learn.Forms.Droid.Renderers;
using Flip2Learn.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NativeMediaView), typeof(NativeMediaViewRenderer))]
namespace Flip2Learn.Forms.Droid.Renderers
{
    public class NativeMediaViewRenderer : ViewRenderer<NativeMediaView, MediaView>
    {
        public NativeMediaViewRenderer(Context context) : base(context)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<NativeMediaView> e)
        {
            try
            {
                base.OnElementChanged(e);
                //call first time for init view
                if (e.NewElement != null && Control == null)
                {
                    var view = new MediaView(Context);
                    view.SetMediaContent((IMediaContent)Element.MediaContent?.NativeContentSource);
                    SetNativeControl(view);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}