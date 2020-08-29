using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Helpers;
using Flip2Learn.Shared.Resources;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Essentials;

namespace Flip2Learn.Shared.Application
{
    /// <summary>
    /// Crossplatform application
    /// </summary>
    public interface ICrossApplication
    {
        Environment Environment { get; }

        string GetString(string key);
        string GetLocale();


        List<Country> GetAllCountries();
    }

    public struct Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }


    public class Environment
    {
        public string AppVersion { get; set; }
        public string OsVersion { get; set; }
        public string DeviceModel { get; set; }
        public long? DeviceMemory { get; set; }
        public Size DeviceScreen { get; set; }
        public string Locale { get; set; }
    }

    public class SimpleTaskResult
    {
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public string LocalizedMessage { get; set; }


        public static SimpleTaskResult Ok() => Ok<SimpleTaskResult>();
        public static SimpleTaskResult Error(Exception exception = null, string localizedMessage = null) => Error<SimpleTaskResult>(exception, localizedMessage);


        public static T Ok<T>() where T : SimpleTaskResult, new()
        {
            return new T()
            {
                IsSuccess = true
            };
        }

        public static T Error<T>(Exception exception = null, string localizedMessage = null) where T : SimpleTaskResult, new()
        {
            return new T()
            {
                IsSuccess = false,
                Exception = exception,
                LocalizedMessage = localizedMessage
            };
        }
    }


    public abstract partial class CrossApplication : ICrossApplication
    {
        public abstract ICrossApplication App { get; }
        public static ICrossApplication instance { get; protected set; }

        private static Environment environment;
        public Environment Environment
        {
            get
            {
                lock (typeof(Environment))
                {
                    if (environment == null)
                    {
                        environment = new Environment()
                        {
                            AppVersion = $"{VersionTracking.CurrentVersion} ({VersionTracking.CurrentBuild})",
                            DeviceModel = DeviceInfo.Model,
                            Locale = "en",//CultureInfo.CurrentCulture.ToString(),
                            OsVersion = DeviceInfo.VersionString,
                            DeviceScreen = new Size()
                            {
                                Width = (int)DeviceDisplay.MainDisplayInfo.Width,
                                Height = (int)DeviceDisplay.MainDisplayInfo.Height
                            }
                        };
                    }

                    return environment;
                }
            }
        }

      
        public virtual ISettings SettingsImplementation => CrossSettings.Current;


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public T GetFromJsonAsset<T>(string assetName)
        {
            var assembly = typeof(CrossApplication).Assembly;
            var resourceName = $"Flip2Learn.Shared.Assets.{assetName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                var serializer = new JsonSerializer();

                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }



        protected CrossApplication()
        {
        }


        public string GetString(string key) => Translator.GetString(key);

        public string GetLocale() => this.Environment.Locale;

        private List<Country> countries;

        public List<Country> GetAllCountries()
        {
            lock (typeof(Country))
            {
                if (countries == null)
                    countries = GetFromJsonAsset<List<Country>>("countries.json");

                return countries;
            }
        }
    }
}
