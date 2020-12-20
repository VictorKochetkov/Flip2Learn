using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Helpers;
using Flip2Learn.Shared.Models;
using Flip2Learn.Shared.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Flip2Learn.Forms.Views
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QuestionCard : ContentView
    {
        public event EventHandler<EventArgs> Clicked = delegate { };
        public event EventHandler<EventArgs> Purchase = delegate { };
        public event EventHandler<EventArgs> MarkAsKnown = delegate { };

        private QuestionDisplay _question;

        public QuestionCard(QuestionDisplay _question)
        {
            InitializeComponent();

            this._question = _question;

            cardFront.Clicked += (s, e) => Clicked(this, new EventArgs());

            cardBackContainer.IsVisible = false;
            cardBackContainer.RotationY = -270;

            cardFront.IsVisible = true;
            cardFront.RotationY = 0;

            cardFront.SizeChanged += (s, e) =>
            {
                cardBack.HeightRequest = cardFront.Height;
            };

            purchase.Clicked += (s, e) =>
            {
                Purchase(this, new EventArgs());
            };



            this.TranslationY = 600;


            title.SetText(_question.CountryName);
            subtitle.SetText(_question.Title);
            flag.SetText(_question.Flag);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<Locale> GetSpeechLocale()
        {
            var locales = await TextToSpeech.GetLocalesAsync();
            return locales.FirstOrDefault(x => x.Language.StartsWith(_question.SpeechLanguage));
        }

        /// <summary>
        /// 
        /// </summary>
        public async void Show()
        {
            TextToSpeech.SpeakAsync(_question.CountryName, new SpeechOptions() { Locale = await GetSpeechLocale() });

            progress.IsVisible = false;
            await this.TranslateTo(0, 0, 400, Easing.CubicOut);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public async void Hide(Action callback)
        {
            const int _delay = 3000;

            progress.IsVisible = true;
            progress.Opacity = 0;
            progress.Progress = 0;

            progress.FadeTo(1, 150);

            uint delay = 20;

            CancellationTokenSource source = new CancellationTokenSource();


            var handler = new EventHandler((s, e) =>
            {
                progress.Progress = 100;
                source.Cancel();
            });

            var handler2 = new EventHandler((s, e) =>
            {
                MarkAsKnown(this, new EventArgs());

                progress.Progress = 100;
                source.Cancel();
            });


            cardBack.Clicked += handler;
            know.Clicked += handler2;



            Device.StartTimer(TimeSpan.FromMilliseconds(delay), () =>
            {
                progress.Progress += 100d * (1d / (_delay / (double)delay));
                return progress.Progress < 100;
            });



            try
            {
                await Task.Delay(_delay, source.Token);
            }
            catch { }



            this.FadeTo(0, 400, Easing.CubicOut);
            await this.TranslateTo(0, -300, 400, Easing.CubicOut);

            progress.IsVisible = false;

            callback();

            cardBack.Clicked -= handler;
            know.Clicked -= handler2;
        }



        public void UpdateResultViews()
        {
            if (_question.ShouldShowAnswer)
            {
                cardBack.Margin = new Thickness(32, 32, 32, 32);
                know.IsVisible = true;
                purchase.IsVisible = false;
                resultTitle.SetText(_question.Capital);
                resultSubtitle.SetText(_question.CountryName);
                flagBack.SetText(_question.Flag);
            }
            else
            {
                cardBack.Margin = new Thickness(32, 32, 32, 64);
                know.IsVisible = false;
                purchase.IsVisible = true;
                resultTitle.SetText(null);
                resultSubtitle.SetText("$=purchase_premium_card$$".Translate());
                flagBack.SetText("🙂");
            }
        }


        public async Task Flip()
        {
            cardBack.HeightRequest = cardFront.Height;
            await cardFront.RotateYTo(-90, 300, Easing.SpringIn);
            cardFront.IsVisible = false;
            cardBackContainer.IsVisible = true;


            UpdateResultViews();


            resultTitle.IsVisible = false;
            resultSubtitle.IsVisible = false;
            flagBack.IsVisible = false;

            cardBackContainer.RotationY = 90;
            cardBackContainer.RotateYTo(0, 300, Easing.SpringOut);

            if (_question.ShouldShowAnswer)
                TextToSpeech.SpeakAsync(_question.Capital.ToString(), new SpeechOptions() { Locale = await GetSpeechLocale() });

            await Task.Delay(10);

            resultTitle.IsVisible = true;
            resultSubtitle.IsVisible = true;
            flagBack.IsVisible = true;
        }
    }
}
