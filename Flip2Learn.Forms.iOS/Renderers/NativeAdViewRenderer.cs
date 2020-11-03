using System;
using System.ComponentModel;
using Flip2Learn.Forms.iOS.Renderers;
using Flip2Learn.Forms.Views;
using Google.MobileAds;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NativeAdFormsView), typeof(NativeAdViewRenderer))]
namespace Flip2Learn.Forms.iOS.Renderers
{
    public class NativeAdViewRenderer : ViewRenderer<NativeAdFormsView, UnifiedNativeAdView>
    {
        public NativeAdViewRenderer()
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
                    Control.NativeAd = (UnifiedNativeAd)Element.NativeAd?.NativeAdSource;
                    break;

                case nameof(Element.Button):
                    Control.CallToActionView = Element.Button.GetRenderer().NativeView;
                    break;

                case nameof(Element.Headline):
                    Control.HeadlineView = Element.Headline.GetRenderer().NativeView;
                    break;

                case nameof(Element.Body):
                    Control.BodyView = Element.Body.GetRenderer().NativeView;
                    break;

                case nameof(Element.Image):
                    Control.ImageView = Element.Image.GetRenderer().NativeView;
                    break;

                case nameof(Element.Advertiser):
                    Control.AdvertiserView = Element.Advertiser.GetRenderer().NativeView;
                    break;

                case nameof(Element.MediaContent):
                    Control.MediaView = (MediaView)Element.MediaContent.GetRenderer().NativeView;
                    break;
            }
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
                    var view = new UnifiedNativeAdView();
                    view.CallToActionView = Element.Button?.GetRenderer()?.NativeView;
                    view.HeadlineView = Element.Headline?.GetRenderer()?.NativeView;
                    view.BodyView = Element.Body?.GetRenderer()?.NativeView;
                    view.ImageView = Element.Image?.GetRenderer()?.NativeView;
                    view.AdvertiserView = Element.Advertiser?.GetRenderer()?.NativeView;
                    var v = Element.MediaContent.GetRenderer()?.NativeView;
                    view.MediaView = (MediaView)Element.MediaContent.GetRenderer()?.NativeView;
                    view.NativeAd = (UnifiedNativeAd)Element.NativeAd?.NativeAdSource;
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
