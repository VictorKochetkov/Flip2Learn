using Flip2Learn.Shared.Application;
using Flip2Learn.Shared.Core;
using Flip2Learn.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flip2Learn.Shared.Helpers
{
    public static class _Helpers
    {
        public static string GetLocalized(this LocalizedString str,bool genetive = false)
        {
            return str.GetLocalized(CrossApplication.instance.Environment.Locale, genetive);
        }

        private static Random rng = new Random();

        /// <summary>
        /// Randomly shuffle this list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="start">Included start index</param>
        /// <param name="end">Included end index</param>
        public static void Shuffle<T>(this IList<T> list, int? start = null, int? end = null)
        {
            if (start == null && end == null)
            {
                start = 0;
                end = list.Count - 1;
            }
            else if (start != null && end != null)
            {
            }
            else
            {

                throw new ArgumentException($"Both [{nameof(start)}] and [{nameof(end)}] have to be specified");
            }


            int n = end.Value + 1;
            while (n > start + 1)
            {
                n--;
                int k = rng.Next(start.Value, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
