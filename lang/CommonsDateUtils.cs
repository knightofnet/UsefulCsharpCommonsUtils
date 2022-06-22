using System;
using UsefulCsharpCommonsUtils.lang.ext;

namespace UsefulCsharpCommonsUtils.lang
{
    /// <summary>
    /// Methods to handle Dates 
    /// </summary>
    public static class CommonsDateUtils
    {

        /// <summary>
        /// Returns the last date of month of a date.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime date)
        {
            DateTime tpDt = date.AddMonths(1);
            return tpDt.AddDays(-tpDt.Day);

        }

        /// <summary>
        /// Returns the first date of month of a date.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime date)
        {

            return date.AddDays(-date.Day + 1);

        }

        /// <summary>
        /// Test if a DateTime object is After an other.
        /// </summary>
        /// <param name="date">the date to test is after</param>
        /// <param name="anotherDate">anotherDate</param>
        /// <returns>true if date is after anotherDate. False if not, or equals</returns>
        public static bool IsAfter(DateTime date, DateTime anotherDate)
        {

            return date.CompareTo(anotherDate) > 0;

        }

        /// <summary>
        /// Test if a DateTime object is before an other.
        /// </summary>
        /// <param name="date">the date to test is before</param>
        /// <param name="anotherDate">anotherDate</param>
        /// <returns>true if date is before anothe rDate. False if not, or equals</returns>
        public static bool IsBefore(DateTime date, DateTime anotherDate)
        {

            return date.CompareTo(anotherDate) < 0;

        }

        private static readonly string[] Sizes = { "ms", "s", "min", "h" };

        public static string HumanReadableTime(long tMs, string format = "{0:0.##} {1}")
        {
            double tDMs = tMs;
            int order = 0;
            while (tDMs >= 1000 && order < Sizes.Length - 1)
            {
                order++;
                tDMs = tDMs / 1000;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", tDMs, Sizes[order]);

        }

        /// <summary>
        /// Returns the earliest date between two dates.
        /// </summary>
        /// <param name="dateA"></param>
        /// <param name="dateB"></param>
        /// <returns></returns>
        public static DateTime MinDateTime(DateTime dateA, DateTime dateB)
        {
            return dateA.IsBefore(dateB) ? dateA : dateB;
        }

        /// <summary>
        /// Returns the most recent date between two dates.
        /// </summary>
        /// <param name="dateA"></param>
        /// <param name="dateB"></param>
        /// <returns></returns>
        public static DateTime MaxDateTime(DateTime dateA, DateTime dateB)
        {
            return dateA.IsBefore(dateB) ? dateB : dateA;
        }

    }
}
