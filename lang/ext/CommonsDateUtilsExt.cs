using System;

namespace UsefulCsharpCommonsUtils.lang.ext
{

    /// <summary>
    /// Extensions methods for DateUtils object.
    /// </summary>
    public static class CommonsDateUtilsExt
    {

        /// <summary>
        /// Returns the last date of month of a date.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return CommonsDateUtils.LastDayOfMonth(date);
        }

        /// <summary>
        /// Returns the first date of month of a date.
        /// </summary>
        /// <param name="date">the date</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return CommonsDateUtils.FirstDayOfMonth(date);
        }

        /// <summary>
        /// Test if a DateTime object is After an other.
        /// </summary>
        /// <param name="date">the date to test is after</param>
        /// <param name="anotherDate">anotherDate</param>
        /// <returns>true if date is after anotehrDate. False if not, or equals</returns>
        public static bool IsAfter(this DateTime dateObj, DateTime dateToTest)
        {

            return CommonsDateUtils.IsAfter(dateObj, dateToTest);

        }

        /// <summary>
        /// Test if a DateTime object is before an other.
        /// </summary>
        /// <param name="date">the date to test is before</param>
        /// <param name="anotherDate">anotherDate</param>
        /// <returns>true if date is before anothe rDate. False if not, or equals</returns>
        public static bool IsBefore(this DateTime dateObj, DateTime dateToTest)
        {

            return CommonsDateUtils.IsBefore(dateObj, dateToTest);

        }

        /// <summary>
        /// Return a new instance of DateTime, with date part (year, month, day) taken form another DateTime. Time preserved from original.
        /// </summary>
        /// <param name="dateTime">the input DateTime</param>
        /// <param name="newDate">the new DateTime to take date part</param>
        /// <returns>A new instance of DateTime, with date part (year, month, day)</returns>
        public static DateTime ChangeDate(this DateTime dateTime, DateTime newDate)
        {
            return new DateTime(
                newDate.Year,
                newDate.Month,
                newDate.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Millisecond,
                dateTime.Kind);
        }


        /// <summary>
        /// Change time part of a DateTime object.
        /// </summary>
        /// <param name="dateTime">input DateTime object</param>
        /// <param name="hours">new hour to set</param>
        /// <param name="minutes">new minute to set</param>
        /// <param name="seconds">new seconde to set</param>
        /// <param name="milliseconds">new millisecond to set</param>
        /// <returns>A new DateTime object, with time part changed</returns>
        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        /// <summary>
        /// Change time part of a DateTime object.
        /// </summary>
        /// <param name="dateTime">input DateTime object</param>
        /// <param name="ts">the TimeSpan object to take time part</param>
        /// <returns>A new DateTime object, with time part changed</returns>
        public static DateTime ChangeTime(this DateTime dateTime, TimeSpan ts)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                ts.Hours,
                ts.Minutes,
                ts.Seconds,
                ts.Milliseconds,
                dateTime.Kind);
        }
    }
}
