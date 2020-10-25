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

namespace Flip2Learn.Forms.Pages
{
    public abstract class AppContentPage : ContentPage
    {
        protected ICrossApplication app => CrossApplication.instance;
    }


    public interface ISprint
    {
        event EventHandler NewQuestion;
        QuestionDisplay Question { get; }
        int QuestionNumber { get; }
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

        int index = 0;
        int currentQuestion = 0;
        private List<Country> currentPart = new List<Country>();
        private List<string> Known = new List<string>();
        private List<Country> countries;

        public QuestionDisplay Question { get; private set; }
        private Country Country => Question.Source;

        private ICrossApplication app => CrossApplication.instance;

        private int known = 0;
        public int QuestionNumber => known;
        public int TotalQuestions => 10;

        public Sprint()
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                Known = realm
                    .All<CountrySnapshot>()
                    .Where(x => x.IsMarkedAsKnown)
                    .ToList()
                    .Select(x => x.Id)
                    .ToList();

                countries = CrossApplication.instance.GetAllCountries().ToList();
            }






        }





        /// <summary>
        /// 
        /// </summary>
        public void MarkAsKnown()
        {
            known++;
            app.MarkAsKnown(Country.NameAsId(), true);
            Known.Add(Country.NameAsId());
        }


        /// <summary>
        /// 
        /// </summary>
        public void NextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= currentPart.Count)
            {
                if (currentPart.All(x => Known.Contains(x.NameAsId())))
                {
                    index++;

                    currentPart = countries
                        .Skip(index * TotalQuestions)
                        .Take(TotalQuestions)
                        .ToList();
                }

                currentPart = currentPart
                    .Where(x => !Known.Contains(x.NameAsId()))
                    .ToList();

                currentQuestion = 0;
                known = 0;
                currentPart.Shuffle();
            }

            Question = new QuestionDisplay(currentPart[currentQuestion]);
            NewQuestion(this, new EventArgs());
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
        private void Game_NewQuestion(object sender, EventArgs e)
        {
            UpdateProgress();
            Show(game.Question);
        }


        /// <summary>
        /// 
        /// </summary>
        void UpdateProgress()
        {
            subtitle.SetText($"{game.QuestionNumber} of {game.TotalQuestions}");

            double value = game.QuestionNumber / (double)game.TotalQuestions;
            progress.Animate("aa", new Animation((a) => { progress.WidthRequest = a; }, progress.Width, progressBackground.Width * value, Easing.CubicOut));
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            container.WidthRequest = Math.Min(400, this.Width);

            app.AppChanged += App_AppChanged;
            //close.Clicked += Close_Clicked;
            settings.Clicked += Settings_Clicked;
            settings.SizeChanged += Settings_SizeChanged;

            if (game == null)
            {
                game = new Sprint();
                game.NewQuestion += Game_NewQuestion;
                game.NextQuestion();
            }

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
            Navigation.PushModalAsync(new SettingsPage());
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
