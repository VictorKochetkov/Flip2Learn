using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Flip2Learn.Shared.Resources
{
    /// <summary>
    /// https://mindofai.github.io/Implementing-Localization-..
    /// </summary>
    public static class Translator
    {
        const string ResourceId = "Flip2Learn.Shared.Resources.AppResources";
        private static ResourceManager resourceManager = new ResourceManager(ResourceId, typeof(Translator).GetTypeInfo().Assembly);

        public static string GetString(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentCulture);
        }
    }

}