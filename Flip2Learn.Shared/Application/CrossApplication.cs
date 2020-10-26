using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Database;
using Flip2Learn.Shared.Helpers;
using Flip2Learn.Shared.Models;
using Flip2Learn.Shared.Resources;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Realms;
using Xamarin.Essentials;

namespace Flip2Learn.Shared.Application
{
    /// <summary>
    /// 
    /// </summary>
    public enum AppChangedType : int
    {
        KnownCountries = 0
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppChangedEventArgs : EventArgs
    {
        public AppChangedType ChangedType { get; set; }
    }


    /// <summary>
    /// Crossplatform application
    /// </summary>
    public interface ICrossApplication
    {
        event EventHandler<AppChangedEventArgs> AppChanged;
        Environment Environment { get; }

        string GetString(string key);
        string GetLocale();


        IReadOnlyList<Country> GetAllCountries();
        IEnumerable<ISelectCountryDisplay> GetSelectCountryList();
        void MarkAsKnown(string countryId, bool known);
        [Obsolete]
        CountrySnapshot FindSnapshotOrCreate(string countryId);
        int GetKnownCountriesCount();
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
        public event EventHandler<AppChangedEventArgs> AppChanged = delegate { };

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


        /// <summary>
        /// 
        /// </summary>
        protected CrossApplication()
        {
            RestoreSnapshots();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key) => Translator.GetString(key);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLocale() => this.Environment.Locale;


        private IReadOnlyList<Country> countries;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<Country> GetAllCountries()
        {
            lock (typeof(Country))
            {
                if (countries == null)
                    countries = GetFromJsonAsset<List<Country>>("countries.json");

                return countries;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void RestoreSnapshots()
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                realm.Write(() =>
                {
                    foreach (var country in GetAllCountries())
                    {
                        var snapshot = realm.Find<CountrySnapshot>(country.NameAsId());

                        if (snapshot == null)
                        {
                            realm.Add(new CountrySnapshot()
                            {
                                Id = country.NameAsId()
                            });
                        }
                    }
                });
            }
        }


        private static Realm realm;
        private static object realmLock = new object();
        private static Realm UIRealm
        {
            get
            {
                lock (realmLock)
                {
                    if (realm == null)
                        realm = RealmHelper.GetRealmInstance();

                    return realm;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [Obsolete]
        public CountrySnapshot FindSnapshotOrCreate(string countryId)
        {
            return UIRealm.Find<CountrySnapshot>(countryId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetKnownCountriesCount()
        {
            int count = 0;

            using (var realm = RealmHelper.GetRealmInstance())
            {
                foreach (var country in GetAllCountries())
                {
                    var snapshot = realm.Find<CountrySnapshot>(country.NameAsId());

                    if (snapshot.IsMarkedAsKnown)
                        count++;
                }
            }

            return count;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private void PostEvent(Action action)
        {
            //TODO thread queue
            action();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="countryId"></param>
        public void MarkAsKnown(string countryId, bool known)
        {
            using (var realm = RealmHelper.GetRealmInstance())
            {
                var snapshot = realm.Find<CountrySnapshot>(countryId);

                realm.Write(() =>
                {
                    snapshot.IsMarkedAsKnown = known;
                });
            }

            PostEvent(() =>
            {
                AppChanged(this, new AppChangedEventArgs()
                {
                    ChangedType = AppChangedType.KnownCountries
                });
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISelectCountryDisplay> GetSelectCountryList()
        {
            return GetAllCountries()
                .OrderBy(x => x.Name.GetLocalized())
                .Select(x => new SelectCountryDisplay(x));
        }
    }
}
