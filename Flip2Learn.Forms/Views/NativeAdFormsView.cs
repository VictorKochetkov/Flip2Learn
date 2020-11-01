using Flip2Learn.Shared.Application;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Views
{
    public class NativeAdFormsView : ContentView
    {
        public static readonly BindableProperty NativeAdProperty = BindableProperty.Create(
                nameof(NativeAd),
                typeof(INativeAd),
                typeof(NativeAdFormsView),
                null);

        public static readonly BindableProperty ButtonProperty = BindableProperty.Create(
                nameof(Button),
                typeof(View),
                typeof(NativeAdFormsView),
                null);

        public static readonly BindableProperty HeadlineProperty = BindableProperty.Create(
                nameof(Headline),
                typeof(View),
                typeof(NativeAdFormsView),
                null);

        public static readonly BindableProperty BodyProperty = BindableProperty.Create(
               nameof(Body),
               typeof(View),
               typeof(NativeAdFormsView),
               null);

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
               nameof(Image),
               typeof(View),
               typeof(NativeAdFormsView),
               null);

        public static readonly BindableProperty AdvertiserProperty = BindableProperty.Create(
               nameof(Advertiser),
               typeof(View),
               typeof(NativeAdFormsView),
               null);

        public INativeAd NativeAd
        {
            get
            {
                return (INativeAd)GetValue(NativeAdProperty);
            }
            set
            {
                SetValue(NativeAdProperty, value);
            }
        }

        public View Button
        {
            get
            {
                return (View)GetValue(ButtonProperty);
            }
            set
            {
                SetValue(ButtonProperty, value);
            }
        }

        public View Headline
        {
            get
            {
                return (View)GetValue(HeadlineProperty);
            }
            set
            {
                SetValue(HeadlineProperty, value);
            }
        }

        public View Body
        {
            get
            {
                return (View)GetValue(BodyProperty);
            }
            set
            {
                SetValue(BodyProperty, value);
            }
        }

        public View Image
        {
            get
            {
                return (View)GetValue(ImageProperty);
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public View Advertiser
        {
            get
            {
                return (View)GetValue(AdvertiserProperty);
            }
            set
            {
                SetValue(AdvertiserProperty, value);
            }
        }

    }
}
