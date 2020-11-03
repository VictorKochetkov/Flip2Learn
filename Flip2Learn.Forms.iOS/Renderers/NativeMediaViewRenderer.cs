using System;
using Flip2Learn.Forms.iOS.Renderers;
using Flip2Learn.Forms.Views;
using Google.MobileAds;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NativeMediaView), typeof(NativeMediaViewRenderer))]
namespace Flip2Learn.Forms.iOS.Renderers
{
    public class NativeMediaViewRenderer : ViewRenderer<NativeMediaView, MediaView>
    {
        public NativeMediaViewRenderer()
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
                    var view = new MediaView();
                    view.MediaContent = (MediaContent)Element.MediaContent?.NativeContentSource;
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
