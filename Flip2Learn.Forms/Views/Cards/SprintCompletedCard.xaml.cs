using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SprintCompletedCard : ContentView
    {
        public event EventHandler NewSprint = delegate { };

        /// <summary>
        /// 
        /// </summary>
        public SprintCompletedCard()
        {
            InitializeComponent();
            this.TranslationY = 600;

            newSprint.Clicked += NewSprint_Clicked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSprint_Clicked(object sender, EventArgs e)
        {
            NewSprint(this, new EventArgs());
        }


        /// <summary>
        /// 
        /// </summary>
        public async void Show()
        {
            await this.TranslateTo(0, 0, 400, Easing.CubicOut);
        }

    }
}