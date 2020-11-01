using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Text;
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
}

