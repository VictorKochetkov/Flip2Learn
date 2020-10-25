using Flip2Learn.Shared.Application;
using Realms;
using Realms.Sync;
using Realms.Sync.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flip2Learn.Shared.Database
{
    public interface IRealmProvider
    {
        Realm Instance { get; }
    }
    public class SimpleRealmProvider : IRealmProvider
    {
        public Realm Instance { get; private set; }

        public SimpleRealmProvider(Realm realm)
        {
            this.Instance = realm;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class RealmHelper
    {
        public const string DEFAULT_REALM_NAME = "default.realm";

        public static Realm GetRealmInstance()
        {
            lock (typeof(RealmHelper))
            {
                return Realm.GetInstance(GetConfig());
            }
        }

        private static RealmConfiguration GetConfig()
        {
            var config = new RealmConfiguration(DEFAULT_REALM_NAME)
            {
                SchemaVersion = 2,
                MigrationCallback = (a, b) =>
                {
                },
            };

            return config;
        }

        /// <summary>
        /// Get realm db size in MB
        /// </summary>
        /// <returns></returns>
        public static double GetDbSizeInMb()
        {
            long db_size = GetDbSize();
            double db_size_mb = db_size / 1024d / 1024d;

            return Math.Round(db_size_mb, 2);
        }

        /// <summary>
        /// Get size of realm db in bytes
        /// </summary>
        /// <returns></returns>
        public static long GetDbSize()
        {
            return new FileInfo(GetDbPath())?.Length ?? 0;
        }

        /// <summary>
        /// Get path to database file
        /// </summary>
        /// <returns></returns>
        public static string GetDbPath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), RealmHelper.DEFAULT_REALM_NAME);
        }


        public static void WriteSafe(this Realm realm, Action action)
        {
            if (realm.IsInTransaction)
                action();
            else
                realm.Write(action);
        }
    }
}
