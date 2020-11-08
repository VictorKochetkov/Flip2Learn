using FFImageLoading.Forms;
using Flip2Learn.Shared.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

public static class UIHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="source"></param>
    public static void SetSource(this CachedImage image, string source)
    {
        image.Source = source;
        image.IsVisible = !string.IsNullOrEmpty(source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="text"></param>
    public static void SetText(this Label label, string text)
    {
        label.Text = text;
        label.IsVisible = !string.IsNullOrEmpty(text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="button"></param>
    /// <param name="text"></param>
    public static void SetText(this Button button, string text)
    {
        button.Text = text;
        button.IsVisible = !string.IsNullOrEmpty(text);
    }


    public static Page CurrentPage => Flip2Learn.Forms.App.Current?.MainPage?.Navigation?.NavigationStack?.LastOrDefault();


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static async Task<SimpleTaskResult> ProgressOverlay(Func<Task<SimpleTaskResult>> task, Action cancel = null, Action accepted = null)
    {
        try
        {
            var result = await task();

            if (result.ShouldShowAlert)
            {
                if (result.TwoOptionsAlert)
                {
                    bool accept = await CurrentPage.DisplayAlert(result.LocalizeTitle, result.LocalizedMessage, result.AcceptText, result.CancelText);

                    if (accept)
                        accepted?.Invoke();
                    else
                        cancel?.Invoke();

                    return result;
                }
                else
                {
                    await CurrentPage.DisplayAlert(result.LocalizeTitle, result.LocalizedMessage, result.CancelText);
                    cancel?.Invoke();
                    return result;
                }
            }

            return result;
        }
        catch (Exception e)
        {
            Debugger.Break();
            return SimpleTaskResult.Error(e);
        }
    }
}

