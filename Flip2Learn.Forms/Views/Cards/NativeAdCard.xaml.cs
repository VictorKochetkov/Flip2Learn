using Flip2Learn.Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms.Views.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NativeAdCard : ContentView
    {
        public event EventHandler Finished = delegate { };

        public NativeAdCard(INativeAd ad)
        {
            InitializeComponent();

            this.TranslationY = 600;

            headline.SetText(ad.Headline);
            body.SetText(ad.Body);
            image.SetSource(ad.ImageUrl);
            button.SetText(ad.Button);
            advertiser.SetText(ad.AdvetiserName);


            adView.Headline = headline;
            adView.Body = body;
            adView.Button = button;
            adView.Image = image;
            adView.Advertiser = advertiser;
            adView.NativeAd = ad;
        }


        /// <summary>
        /// 
        /// </summary>
        public async void Show()
        {
            await this.TranslateTo(0, 0, 400, Easing.CubicOut);

            var timer = new Timer(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(10));
            timer.Start();
            timer.Tick += (s, e) =>
            {
                progress.Progress = timer.Progress * 100 ?? 0;
            };
            timer.Finished += async (s, e) =>
            {
                await Hide();
                Finished(this, EventArgs.Empty);
            };
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task Hide()
        {
            this.FadeTo(0, 400, Easing.CubicOut);
            await this.TranslateTo(0, -300, 400, Easing.CubicOut);
        }



        /// <summary>
        /// 
        /// </summary>
        public class Timer
        {
            /// <summary>
            /// 
            /// </summary>
            public event EventHandler Finished = delegate { };

            /// <summary>
            /// 
            /// </summary>
            public event EventHandler Tick = delegate { };

            /// <summary>
            /// 
            /// </summary>
            public bool IsFinished => start != null && DateTime.Now >= start + duration;


            /// <summary>
            /// 
            /// </summary>
            public double? Progress => start != null ? Math.Min(1, (DateTime.Now - start).Value.TotalMilliseconds / duration.TotalMilliseconds) : (double?)null;


            private DateTime? start;
            private readonly TimeSpan duration;
            private readonly TimeSpan interval;


            /// <summary>
            /// 
            /// </summary>
            /// <param name="interval"></param>
            /// <param name="duration"></param>
            public Timer(TimeSpan interval, TimeSpan duration)
            {
                this.interval = interval;
                this.duration = duration;
            }


            /// <summary>
            /// 
            /// </summary>
            public void Start()
            {
                start = DateTime.Now;

                Device.StartTimer(interval, () =>
                {
                    bool finished = IsFinished;

                    Tick(this, EventArgs.Empty);

                    if (finished)
                        Finished(this, EventArgs.Empty);

                    return !finished;
                });
            }
        }
    }
}