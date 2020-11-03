using System;
using Flip2Learn.Shared.Application;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Views
{
    public class NativeMediaView : View
    {
        public static readonly BindableProperty MediaContentProperty = BindableProperty.Create(
                nameof(MediaContent),
                typeof(IMediaContent),
                typeof(NativeMediaView),
                null);

        public IMediaContent MediaContent
        {
            get
            {
                return (IMediaContent)GetValue(MediaContentProperty);
            }
            set
            {
                SetValue(MediaContentProperty, value);
            }
        }
    }
}
