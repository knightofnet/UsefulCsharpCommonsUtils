using System.Linq;

namespace UsefulCsharpCommonsUtils.lang.ext
{
    /// <summary>
    /// Static method for Strings.
    /// </summary>
    public static  class CommonsStringUtilsExt
    {

        public static bool Matches(this string text, string regex)
        {
            return CommonsStringUtils.Matches(text, regex);
        }

        /// <summary>
        /// Tests if a string contained all others (toSearch)
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="toSearch"></param>
        /// <returns></returns>
        public static bool ContainsMultipleWithAnd(this string haystack, string[] toSearch)
        {
            if (haystack == null) return false;
            if (toSearch == null || !toSearch.Any()) return false;

            return toSearch.All(haystack.Contains);
        }

        /// <summary>
        /// Tests if a string contained on of the others (toSearch)
        /// </summary>
        /// <param name="haystack"></param>
        /// <param name="toSearch"></param>
        /// <returns></returns>
        public static bool ContainsMultipleWithOr(this string haystack, string[] toSearch)
        {
            if (haystack == null) return false;
            if (toSearch == null || !toSearch.Any()) return false;

            return toSearch.Any(haystack.Contains);
        }

    }
}
