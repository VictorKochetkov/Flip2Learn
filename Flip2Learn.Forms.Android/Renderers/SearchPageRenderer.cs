using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views.InputMethods;
using Android.Widget;
using Flip2Learn.Forms.Droid.Renderers;
using Flip2Learn.Forms.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

///
///https://codetraveler.io/2019/10/05/adding-a-search-bar-to-xamarin-forms-navigationpage/
///

[assembly: ExportRenderer(typeof(SettingsPage), typeof(SearchPageRenderer))]
namespace Flip2Learn.Forms.Droid.Renderers
{
    public class SearchPageRenderer : PageRenderer
    {
        public SearchPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (Element is ISearchPage && Element is Page page && page.Parent is NavigationPage navigationPage)
            {
                //Workaround to re-add the SearchView when navigating back to an ISearchPage, because Xamarin.Forms automatically removes it
                navigationPage.Popped += HandleNavigationPagePopped;
                navigationPage.PoppedToRoot += HandleNavigationPagePopped;
            }
        }


        //Adding the SearchBar in OnSizeChanged ensures the SearchBar is re-added after the device is rotated, because Xamarin.Forms automatically removes it
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            if (Element is ISearchPage && Element is Page page && page.Parent is NavigationPage navigationPage && navigationPage.CurrentPage is ISearchPage)
            {
                AddSearchToToolbar(page.Title);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (GetToolbar() is Toolbar toolBar)
                toolBar.Menu?.RemoveItem(Resource.Menu.main_menu);

            base.Dispose(disposing);
        }

        //Workaround to re-add the SearchView when navigating back to an ISearchPage, because Xamarin.Forms automatically removes it
        void HandleNavigationPagePopped(object sender, NavigationEventArgs e)
        {
            if (sender is NavigationPage navigationPage
                && navigationPage.CurrentPage is ISearchPage)
            {
                AddSearchToToolbar(navigationPage.CurrentPage.Title);
            }
        }

        void AddSearchToToolbar(string pageTitle)
        {
            if (GetToolbar() is Toolbar toolBar && toolBar.Menu?.FindItem(Resource.Id.action_search)?.ActionView?.JavaCast<SearchView>().GetType() != typeof(SearchView))
            {
                toolBar.Title = pageTitle;
                toolBar.InflateMenu(Resource.Menu.main_menu);

                if (toolBar.Menu?.FindItem(Resource.Id.action_search)?.ActionView?.JavaCast<SearchView>() is SearchView searchView)
                {
                    searchView.QueryTextChange += HandleQueryTextChange;
                    searchView.ImeOptions = (int)ImeAction.Search;
                    searchView.QueryHint = (Element as ISearchPage)?.SearchPlaceholder ?? "Search";
                    searchView.InputType = (int)InputTypes.TextVariationFilter;
                    searchView.MaxWidth = int.MaxValue; //Set to full width - http://stackoverflow.com/questions/31456102/searchview-doesnt-expand-full-width

                    try
                    {
                        int id = searchView.Context.Resources.GetIdentifier("search_src_text", "id", searchView.Context.PackageName);
                        TextView textView = searchView.FindViewById<TextView>(id);
                        textView?.SetHintTextColor(ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(searchView.Context, Color.Gray.ToAndroid()))));
                    }
                    catch { }
                }
            }
        }


        private void HandleQueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (Element is ISearchPage searchPage)
                searchPage.OnSearchBarTextChanged(e.NewText);
        }


        Toolbar GetToolbar() => Xamarin.Essentials.Platform.CurrentActivity.FindViewById<Toolbar>(Resource.Id.toolbar);
    }
}