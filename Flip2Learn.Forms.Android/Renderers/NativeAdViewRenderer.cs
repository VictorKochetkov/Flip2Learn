using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads.Formats;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Flip2Learn.Forms.Droid.Renderers;
using Flip2Learn.Forms.Views;
using Flip2Learn.Shared.Application;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(NativeAdFormsView), typeof(NativeAdViewRenderer))]
namespace Flip2Learn.Forms.Droid.Renderers
{
    public class NativeAdViewRenderer : ViewRenderer<NativeAdFormsView, UnifiedNativeAdView>
    {
        public NativeAdViewRenderer(Context context) : base(context)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(Element.NativeAd):
                    Control.SetNativeAd((UnifiedNativeAd)Element.NativeAd?.NativeAdSource);
                    break;

                case nameof(Element.Button):
                    Control.CallToActionView = Element.Button.GetRenderer().View;
                    break;

                case nameof(Element.Headline):
                    Control.HeadlineView = Element.Headline.GetRenderer().View;
                    break;

                case nameof(Element.Body):
                    Control.BodyView = Element.Body.GetRenderer().View;
                    break;

                case nameof(Element.Image):
                    Control.ImageView = Element.Image.GetRenderer().View;
                    break;

                case nameof(Element.Advertiser):
                    Control.AdvertiserView = Element.Advertiser.GetRenderer().View;
                    break;

                case nameof(Element.MediaContent):
                    Control.MediaView = (MediaView)Element.MediaContent.GetRenderer().View;
                    break;
            }
        }


        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Control.CallToActionView = Element.Button?.GetRenderer()?.View;
            Control.HeadlineView = Element.Headline?.GetRenderer()?.View;
            Control.BodyView = Element.Body?.GetRenderer()?.View;
            Control.ImageView = Element.Image?.GetRenderer()?.View;
            Control.AdvertiserView = Element.Advertiser?.GetRenderer()?.View;
            Control.MediaView = (Element.MediaContent?.GetRenderer() as NativeMediaViewRenderer)?.Control;
            Control.SetNativeAd((UnifiedNativeAd)Element.NativeAd?.NativeAdSource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<NativeAdFormsView> e)
        {
            try
            {

                base.OnElementChanged(e);
                //call first time for init view
                if (e.NewElement != null && Control == null)
                {
                    var view = new UnifiedNativeAdView(Context);
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