using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;
using XF.Material.Forms.Resources;

namespace Flip2Learn.Forms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CircleProgressView : StackLayout
    {
        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(
            nameof(Progress),
            typeof(double),
            typeof(CircleProgressView),
            0d,
            propertyChanged: OnProgressPropertyChanged);


        public static readonly BindableProperty ShowTextProperty = BindableProperty.Create(
            nameof(ShowText),
            typeof(bool),
            typeof(CircleProgressView),
            false,
            propertyChanged: OnShowTextPropertyChanged);

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            nameof(Color),
            typeof(Color),
            typeof(CircleProgressView),
            Color.White,
            propertyChanged: OnColorPropertyChanged);

        public static readonly BindableProperty HideBackgroundStrokeProperty = BindableProperty.Create(
            nameof(HideBackgroundStroke),
            typeof(bool),
            typeof(CircleProgressView),
            false,
            propertyChanged: OnHideBackgroundStrokePropertyChanged);

        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(
            nameof(Stroke),
            typeof(double),
            typeof(CircleProgressView),
            8d,
            propertyChanged: OnStrokePropertyChanged);

        public static readonly BindableProperty AnimatedProperty = BindableProperty.Create(
            nameof(Animate),
            typeof(bool),
            typeof(CircleProgressView),
            false,
            propertyChanged: OnAnimatedPropertyChanged);


        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public bool ShowText
        {
            get => (bool)GetValue(ShowTextProperty);
            set => SetValue(ShowTextProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public bool HideBackgroundStroke
        {
            get => (bool)GetValue(HideBackgroundStrokeProperty);
            set => SetValue(HideBackgroundStrokeProperty, value);
        }

        public double Stroke
        {
            get => (double)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public bool Animate
        {
            get => (bool)GetValue(AnimatedProperty);
            set => SetValue(AnimatedProperty, value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnProgressPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            var value = (double)newValue;

            view.progress.Text = $"{(int)value}%";
            view.InvalidateLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnShowTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            view.progress.IsVisible = (bool)newValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            view.UpdatePaint();
            view.InvalidateLayout();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnHideBackgroundStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            view.UpdatePaint();
            view.InvalidateLayout();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            view.UpdatePaint();
            view.InvalidateLayout();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnAnimatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CircleProgressView)bindable;
            view.InvalidateLayout();
        }





        public CircleProgressView()
        {
            UpdatePaint();
            InitializeComponent();
            canvas.PaintSurface += Canvas_PaintSurface;

            if (Device.RuntimePlatform == Device.iOS)
                k = 1;
            else
                k = 1;
        }



        protected override void InvalidateLayout()
        {
            base.InvalidateLayout();
            canvas.InvalidateSurface();
        }


        private void UpdatePaint()
        {
            paint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                StrokeWidth = (float)Stroke,
                IsAntialias = true,
            };

            paintBackground = paint.Clone();
            paintBackground.Color = new SKColor(paintBackground.Color.Red, paintBackground.Color.Green, paintBackground.Color.Blue, (byte)50);
        }


        private SKRect rect;
        private SKPaint paint;
        private SKPaint paintBackground;
        private float _progress = 0;
        private float _rotate = 0;
        const int padding = 2;
        private readonly float k = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Canvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear();

            rect.Size = new SKSize(e.Info.Width - paint.StrokeWidth - padding, e.Info.Height - paint.StrokeWidth - padding);
            rect.Location = new SKPoint(paint.StrokeWidth / 2, paint.StrokeWidth / 2);

            float value = (float)(Progress / 100d * 360d) * (float)Easing.CubicInOut.Ease(_progress);
            float _startAngle = -90 + (360 - value) + _rotate;
            float _sweepAngle = 360 - (360 - value);

            SKPath path = new SKPath();
            path.AddArc(rect, _startAngle, _sweepAngle);

            SKPath pathBackground = new SKPath();
            pathBackground.AddArc(rect, 0, 360);

            canvas.DrawPath(path, paint);
            canvas.DrawPath(pathBackground, paintBackground);


            _progress += 0.05f * k;
            _progress = Math.Min(1, _progress);

            if (Animate)
                _rotate += 8f * k;

            if ((Animate && Progress > 0 && Progress < 100) || (_progress > 0 && _progress < 1))
            {
                await Task.Delay((int)(20 * k));
                InvalidateLayout();
            }

        }

    }
}