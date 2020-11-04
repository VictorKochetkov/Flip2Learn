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
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (app.IsPurchased == true)
            {
                purchased.IsVisible = true;
                purchase.IsVisible = false;
                restore.IsVisible = false;
                feature1.IsVisible = false;
                feature2.IsVisible = false;
                feature3.IsVisible = false;
                feature1_purchased.IsVisible = true;
                feature2_purchased.IsVisible = true;
                feature3_purchased.IsVisible = true;
            }
            else
            {
                purchased.IsVisible = false;
                purchase.IsVisible = true;
                restore.IsVisible = true;
                feature1.IsVisible = true;
                feature2.IsVisible = true;
                feature3.IsVisible = true;
                feature1_purchased.IsVisible = false;
                feature2_purchased.IsVisible = false;
                feature3_purchased.IsVisible = false;
            }
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