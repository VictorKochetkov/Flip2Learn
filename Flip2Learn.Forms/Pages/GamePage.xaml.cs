using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flip2Learn.Forms.Views;
using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Helpers;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Models;
using Xamarin.Forms;
using FFImageLoading.Svg.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Linq;
using Flip2Learn.Shared.Database;
using Realms;
using Flip2Learn.Forms.Views.Cards;
using System.Threading;

namespace Flip2Learn.Forms.Pages
{
    public abstract class AppContentPage : ContentPage
    {
        protected ICrossApplication app => CrossApplication.instance;
    }


    public interface ISprint
    {
        event EventHandler NewQuestion;
        event EventHandler Finished;
        QuestionDisplay Question { get; }
        int QuestionIndex { get; }
        int TotalQuestions { get; }
        void NextQuestion();
        void MarkAsKnown();
    }


    /// <summary>
    /// 
    /// </summary>
    public class Sprint : ISprint
    {
        public event EventHandler NewQuestion = delegate { };
        public event EventHandler Finished = delegate { };

        /// <summary>
        /// 
        /// </summary>
        public QuestionDisplay Question { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private ICrossApplication app => CrossApplication.instance;

        /// <summary>
        /// 
        /// </summary>
        public int QuestionIndex { get; private set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public int TotalQuestions => 10;

        /// <summary>
        /// 
        /// </summary>
        private readonly IReadOnlyList<Country> countries;

        /// <summary>
        /// 
        /// </summary>
        public Sprint()
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                countries = CrossApplication.instance
                    .GetAllCountries()
                    .OrderBy(x => x.Complexity)
                    .Where(x => realm.Find<CountrySnapshot>(x.NameAsId()).IsMarkedAsKnown != true)
                    .Take(20)
                    .Shuffle()
                    .Take(TotalQuestions)
                    .ToList();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void MarkAsKnown()
        {
            app.MarkAsKnown(countries[QuestionIndex].NameAsId(), true);
        }


        /// <summary>
        /// 
        /// </summary>
        public void NextQuestion()
        {
            if (QuestionIndex + 1 == countries.Count)
            {
                Finished(this, new EventArgs());
            }
            else
            {
                QuestionIndex++;
                Question = new QuestionDisplay(countries[QuestionIndex]);
                NewQuestion(this, new EventArgs());
            }
        }
    }




    /// <summary>
    /// 
    /// </summary>
    public partial class GamePage : AppContentPage
    {
        ISprint game;

        public GamePage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (svg.Source == null)
            {
                svg.WidthRequest = width;
                svg.HeightRequest = height;
                svg.Source = "countries_background_pattern";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Game_NewQuestion(object sender, EventArgs e)
        {
            if (ShouldShowAd())
                await ShowAd();

            UpdateProgress();
            Show(game.Question);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ShouldShowAd()
        {
            if (app.LoadedAd == null)
                return false;

            if (game.QuestionIndex != 6)
                return false;

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        public class Waiter
        {
            private readonly AutoResetEvent r;

            public Waiter()
            {
                r = new AutoResetEvent(false);
            }

            public void Done()
            {
                r.Set();
            }

            public async Task WaitAsync()
            {
                await Task.Run(() =>
                {
                    r.WaitOne();
                });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private async Task ShowAd()
        {
            var waiter = new Waiter();

            var card = new NativeAdCard(app.LoadedAd);
            container.Children.Add(card, xConstraint: null);
            card.Show();

            card.Finished += (s, e) =>
            {
                container.Children.Remove(card);

                app.LoadAd(force: true);
                waiter.Done();
            };

            card.DisableAds += async (s, e) =>
            {
                await app.Purchase();
            };

            await waiter.WaitAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Game_Finished(object sender, EventArgs e)
        {
            var card = new SprintCompletedCard();
            container.Children.Add(card, xConstraint: null);
            card.Show();

            card.NewSprint += (s, e2) =>
            {
                container.Children.Remove(card);
                game = null;
                NewSprint();
            };
        }


        /// <summary>
        /// 
        /// </summary>
        void NewSprint()
        {
            if (game == null)
            {
                game = new Sprint();
                game.NewQuestion += Game_NewQuestion;
                game.Finished += Game_Finished;
                game.NextQuestion();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        void UpdateProgress()
        {
            subtitle.SetText($"{game.QuestionIndex + 1} of {game.TotalQuestions}");

            double value = game.QuestionIndex / (double)game.TotalQuestions;
            progress.Animate("aa", new Animation((a) => { progress.WidthRequest = a; }, progress.Width, progressBackground.Width * value, Easing.CubicOut));
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            app.LoadAd();
            container.WidthRequest = Math.Min(400, this.Width);

            app.AppChanged += App_AppChanged;
            //close.Clicked += Close_Clicked;
            settings.Clicked += Settings_Clicked;
            settings.SizeChanged += Settings_SizeChanged;

            NewSprint();

            UpdateTitle();
            UpdateKnownCountriesCount();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_AppChanged(object sender, AppChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                switch (e.ChangedType)
                {
                    case AppChangedType.KnownCountries:
                        UpdateKnownCountriesCount();
                        break;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_SizeChanged(object sender, EventArgs e)
        {
            UpdateTitle();
        }


        /// <summary>
        /// 
        /// </summary>
        void UpdateTitle()
        {
            var safeInsets = On<iOS>().SafeAreaInsets();

            var m = appBar.Margin;
            m.Top = safeInsets.Top;
            appBar.Margin = m;

            var p = titleContainer.Padding;
            p.Left = settings.Width;
            titleContainer.Padding = p;
        }


        /// <summary>
        /// 
        /// </summary>
        void UpdateKnownCountriesCount()
        {
            knownCountries.SetText($"✔️ {app.GetKnownCountriesCount()}");
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            app.AppChanged -= App_AppChanged;
            //close.Clicked -= Close_Clicked;
            settings.Clicked -= Settings_Clicked;
            settings.SizeChanged -= Settings_SizeChanged;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new SettingsPage());
            Navigation.PushAsync(new SettingsPage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="question"></param>
        void Show(QuestionDisplay question)
        {
            var card = new QuestionCard(question);


            //container.Children.Add(card,
            //    Constraint.RelativeToParent((_parent) => (_parent.Width / 2) - (card.Width / 2)),
            //    Constraint.RelativeToParent((_parent) => _parent.Height - card.Height)
            //    );

            container.Children.Add(card, xConstraint: null);

            card.MarkAsKnown += (s, e) =>
            {
                game.MarkAsKnown();
            };

            card.Clicked += async (s, e) =>
            {
                await card.Flip();

                card.Hide(() =>
                {
                    container.Children.Remove(card);
                    game.NextQuestion();
                });

            };

            card.Show();
        }
    }
}
