using System;
using Flip2Learn.Forms.iOS.Renderers;
using Flip2Learn.Forms.Pages;
using UIKit;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(AppContentPage), typeof(AppContentPageRenderer))]
namespace Flip2Learn.Forms.iOS.Renderers
{
    public class AppContentPageRenderer : PageRenderer
    {
        public override void WillMoveToParentViewController(UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            (this.Element as AppContentPage)?.OnFirstAppearing();
        }
    }
}
