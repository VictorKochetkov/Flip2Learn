using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeaturesPage : AppContentPage
    {
        public FeaturesPage()
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
        private async void Purchase_Clicked(object sender, EventArgs e)
        {
            var result = await app.Purchase();

            if (result.IsSuccess)
                await this.Navigation.PopAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Restore_Clicked(object sender, EventArgs e)
        {
            var result = await app.RestorePurchase();

            if (result.IsSuccess)
                await this.Navigation.PopAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void close_Clicked(System.Object sender, System.EventArgs e)
        {
            await this.Navigation.PopAsync();
        }
    }
}