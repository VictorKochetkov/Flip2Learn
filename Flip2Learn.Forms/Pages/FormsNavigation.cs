using System;
using Flip2Learn.Shared.Application;

namespace Flip2Learn.Forms.Pages
{
    public class FormsNavigation : INavigation
    {
        public async void Purchase()
        {
            await UIHelper.CurrentPage.Navigation.PushAsync(new FeaturesPage());
        }
    }
}
