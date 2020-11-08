using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCountryCell : BaseRowCell<ISelectCountryDisplay>
    {
        protected override Label Title => title;
        protected override Label Subtitle => subtitle;

        private ICrossApplication app => CrossApplication.instance;


        public SelectCountryCell()
        {
            InitializeComponent();

            this.Tapped += (s, e) =>
            {
                selected.IsChecked = !selected.IsChecked;
            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selected_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            app.MarkAsKnown(Model.Id, e.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void UpdateViews()
        {
            base.UpdateViews();

            parent.Text = Model.ParentCountry;

            selected.CheckedChanged -= Selected_CheckedChanged;

            flag.Text = Model.Flag;
            selected.IsChecked = Model.IsKnown;

            selected.CheckedChanged += Selected_CheckedChanged;
        }
    }
}
