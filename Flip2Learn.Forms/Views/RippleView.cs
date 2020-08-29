using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace Flip2Learn.Forms.Views
{
    public class RippleView : MaterialCard
    {
        public static readonly BindableProperty IsCircledProperty = BindableProperty.Create(
                nameof(IsCircled),
                typeof(bool),
                typeof(RippleView),
                false);

        public bool IsCircled
        {
            get
            {
                return (bool)GetValue(IsCircledProperty);
            }
            set
            {
                SetValue(IsCircledProperty, value);
            }
        }


        public RippleView()
        {
            IsClickable = true;
            BackgroundColor = Color.Transparent;
            Margin = new Thickness(0);
            Padding = new Thickness(0);
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            CornerRadius = Math.Max(0, (float)Math.Min(width, height) / 2f);
        }

    }
}
