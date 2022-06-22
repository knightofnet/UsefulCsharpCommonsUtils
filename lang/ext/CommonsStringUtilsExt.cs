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

    }
}
