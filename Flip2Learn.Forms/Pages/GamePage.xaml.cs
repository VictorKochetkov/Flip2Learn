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

namespace Flip2Learn.Forms.Pages
{
    public abstract class AppContentPage : ContentPage
    {
        protected ICrossApplication app => CrossApplication.instance;
    }


    public interface IGameNEW
    {
        event EventHandler NewQuestion;
        QuestionDisplay Question { get; }
        void NextQuestion();
        void MarkAsKnown();
    }

    public class GameNEW : IGameNEW
    {
        public event EventHandler NewQuestion = delegate { };

        int index = 0;
        int currentQuestion = 0;
        private List<Country> currentPart = new List<Country>();
        private List<string> Known = new List<string>();
        private List<Country> countries;

        public QuestionDisplay Question { get; private set; }
        private Country Country => Question.Source;

        public GameNEW()
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                Known = realm
                    .All<CountryData>()
                    .Where(x => x.IsMarkedAsKnown)
                    .ToList()
                    .Select(x => x.Id)
                    .ToList();

                countries = CrossApplication.instance.GetAllCountries().ToList();
            }

        }


        public void MarkAsKnown()
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                var _country = realm.Find<CountryData>(Country.NameAsId());

                if (_country == null)
                {
                    _country = new CountryData()
                    {
                        Id = Country.NameAsId(),
                    };
                }

                realm.Write(() =>
                {
                    _country.IsMarkedAsKnown = true;
                    realm.Add(_country, update: true);
                });

                Known.Add(Country.NameAsId());
            }
        }


        public void NextQuestion()
        {
            currentQuestion++;

            if (currentQuestion >= currentPart.Count)
            {
                if (currentPart.All(x => Known.Contains(x.NameAsId())))
                {
                    index++;

                    currentPart = countries
                        .Skip(index * 10)
                        .Take(10)
                        .ToList();
                }

                currentPart = currentPart
                    .Where(x => !Known.Contains(x.NameAsId()))
                    .ToList();

                currentQuestion = 0;
                currentPart.Shuffle();
            }

            Question = new QuestionDisplay(currentPart[currentQuestion]);
            NewQuestion(this, new EventArgs());
        }
    }


    public partial class GamePage : AppContentPage
    {
        IGameNEW game;

        public GamePage()
        {
            InitializeComponent();
        }


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

        int count = 0;

        private void Game_NewQuestion(object sender, EventArgs e)
        {
            UpdateProgress();
            Show(game.Question);
        }


        void UpdateProgress()
        {
            double value = count / 10d;
            progress.Animate("aa", new Animation((a) => { progress.WidthRequest = a; }, progress.Width, progressBackground.Width * value, Easing.CubicOut));
            count++;
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            container.WidthRequest = Math.Min(400, this.Width);

            close.Clicked += Close_Clicked;

            game = new GameNEW();
            game.NewQuestion += Game_NewQuestion;
            game.NextQuestion();

            UpdateViews();
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateViews()
        {
            var safeInsets = On<iOS>().SafeAreaInsets();

            var m = appBar.Margin;
            m.Top = safeInsets.Top;
            appBar.Margin = m;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            close.Clicked -= Close_Clicked;
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            OnBackButtonPressed();
        }

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
