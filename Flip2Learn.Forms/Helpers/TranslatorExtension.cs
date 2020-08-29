using System;
using Flip2Learn.Shared.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Flip2Learn.Forms.Helpers
{
    /// <summary>
    /// https://mindofai.github.io/Implementing-Localization-..
    /// </summary>
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            return Translator.GetString(Text);
        }
    }
}