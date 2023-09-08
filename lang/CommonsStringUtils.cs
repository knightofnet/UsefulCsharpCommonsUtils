using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UsefulCsharpCommonsUtils.lang
{
    /// <summary>
    /// Static method for Strings.
    /// </summary>
    public static class CommonsStringUtils
    {
        /// <summary>
        /// Characters allowed in a Windows filename.
        /// </summary>
        public const string AUTH_FILENAME_CHARS =
    "ABCDEFGHIJKMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-+&é()è_ç[]{}=^¨,;§!µ%ù£$€&~²";
        private static List<string> _listRandomString;


        /// <summary>
        /// Returns a substring of the specified length from the specified start index of the given string.
        /// If the string is null or the start index is greater than or equal to the length of the string, null is returned.
        /// If the specified length extends beyond the end of the string, the substring from the start index to the end of the string is returned.
        /// </summary>
        /// <param name="str">The string to extract the substring from.</param>
        /// <param name="startIndex">The zero-based starting character position of the substring.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A substring of the specified length from the specified start index of the given string, or null if the string is null or the start index is greater than or equal to the length of the string.</returns>
        /// <xml:lang lang="fr">Une sous-chaîne de la longueur spécifiée à partir de l'index de départ spécifié de la chaîne donnée, ou null si la chaîne est nulle ou l'index de départ est supérieur ou égal à la longueur de la chaîne.</xml:lang>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified length is negative.</exception>
        public static string SafeSubstring(string str, int startIndex, int length)
        {
            if (str == null || startIndex >= str.Length) return null;

            return startIndex + length >= str.Length ?
                str.Substring(startIndex)
                : str.Substring(startIndex, length);
        }

        /// <summary>
        /// This method divides a string into substrings of specified length.
        /// </summary>
        /// <param name="str">The string to be divided.</param>
        /// <param name="len">The length of the substrings.</param>
        /// <returns>An array of substrings of specified length.</returns>
        /// <xml:lang lang="fr">Cette méthode divise une chaîne de caractères en sous-chaînes de longueur spécifiée.</xml:lang>
        public static string[] SubstringsByLen(string str, int len)
        {
            List<string> strRed = new List<string>(1);

            string strTemp = str;
            while (strTemp.Length > len)
            {
                string s = strTemp.Substring(0, len);
                strRed.Add(s);
                strTemp = strTemp.Substring(len);
            }
            strRed.Add(strTemp);

            return strRed.ToArray();

        }

        /// <summary>
        /// Split a string by another string
        /// </summary>
        /// <param name="strToSplit">the string to split</param>
        /// <param name="splitStr">the string use to split</param>
        /// <returns>an array of strings splitted</returns>
        public static string[] Split(string strToSplit, string splitStr)
        {
            return strToSplit.Split(new[] { splitStr }, StringSplitOptions.None);
        }

        /// <summary>
        /// Capitalize the first letter and lower the others.
        /// </summary>
        /// <param name="str">this string</param>
        /// <returns>The string with the first letter upped and lowered the others</returns>
        public static string FirstUpperOtherLower(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            string fl = "";
            if (str.Length >= 1)
            {
                fl = str.Substring(0, 1).ToUpper();
            }

            if (str.Length >= 2)
            {
                fl += str.Substring(1).ToLower();
            }

            return fl;
        }


        /// <summary>
        /// Test if a string is empty.
        /// </summary>
        /// <param name="str">the string to test</param>
        /// <returns>true if null, is empty (length = 0 or equals "")</returns>
        public static bool IsEmpty(string str)
        {
            if (str == null)
            {
                return true;
            }
            return str.Length == 0 || str == "";
        }



        /// <summary>
        /// Truncate the string with limit lenght
        /// </summary>
        /// <param name="str">the string to truncate</param>
        /// <param name="limit">le lenght to truncate</param>
        /// <returns>Returns the truncated string, or the complete string if the limit is greater than or equal to the length of the input string.</returns>
        public static string Truncate(string str, int limit)
        {
            if (str == null)
            {
                return null;
            }

            if (limit == 0)
            {
                return "";
            }

            if (limit > 0)
            {
                return str.Substring(0, str.Length < limit ? str.Length : limit);
            }

            return TruncRight(str, limit);
        }

        /// <summary>
        /// Trunc a string from the right if string's lenght greater than defined lenght. A suffix can be use for indicate trunc.
        /// </summary>
        /// <param name="str">The string to truncate</param>
        /// <param name="maxLenght">The length from which to truncate</param>
        /// <param name="suffix">The suffix to indicate truncate. Default=""</param>
        /// <returns>The string with truncate</returns>
        public static string TruncRight(string str, int maxLenght, string suffix = "")
        {
            int start = maxLenght < str.Length ? str.Length - maxLenght : 0;
            int lenght = str.Length - start;

            if (start == 0)
            {
                return str;
            }
            return suffix + str.Substring(start, lenght);
        }


        /// <summary>
        /// Replace the first occurence of a substring in a string.
        /// </summary>
        /// <param name="str">The string in which to search</param>
        /// <param name="search">The substring to search for</param>
        /// <param name="replace">The replacement</param>
        /// <returns>The string with replacement done if exists.</returns>
        public static string ReplaceFirst(string str, string search, string replace)
        {
            int pos = str.IndexOf(search);
            if (pos < 0)
            {
                return str;
            }
            return str.Substring(0, pos) + replace + str.Substring(pos + search.Length);
        }

        /// <summary>
        /// Remove diacritics accent in a string.
        /// Source : https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        /// </summary>
        /// <param name="text">the string</param>
        /// <returns>the string without diacritics accents</returns>
        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Removes characters in a string from a set of characters (placed in a string).
        /// </summary>
        /// <param name="str">The string in which to work</param>
        /// <param name="charList">the set of characters</param>
        /// <returns></returns>
        public static string RemoveCharDifferentThan(string str, string charList)
        {
            StringBuilder retStr = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char s = str[i];
                if (charList.Contains(s))
                    retStr.Append(s);
            }
            return retStr.ToString();
        }

        /// <summary>
        /// Removes characters in a string from a set of characters (placed in a string).
        /// </summary>
        /// <param name="str">The string in which to work</param>
        /// <param name="charList">the set of characters</param>
        /// <returns></returns>
        public static string RemoveChar(string str, string charList)
        {
            StringBuilder retStr = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char s = str[i];
                if (!charList.Contains(s))
                    retStr.Append(s);
            }
            return retStr.ToString();
        }

        /// <summary>
        /// Returns a string if not null, or a default text if not.
        /// </summary>
        /// <param name="text">the text</param>
        /// <param name="defaut">default string</param>
        /// <param name="isTestEmpty"></param>
        /// <returns></returns>
        public static string TextOrDefault(string text, string defaut, bool isTestEmpty = false)
        {
            if (text == null || (isTestEmpty && IsEmpty(text)))
            {
                return defaut;
            }

            return text;
        }

        /// <summary>
        /// Tells whether or not this string matches the given regular expression.
        /// </summary>
        /// <param name="text">the input string to test</param>
        /// <param name="regex">the regular expression to which this string is to be matched</param>
        /// <returns>true if, and only if, this string matches the given regular expression</returns>
        public static bool Matches(string text, string regex)
        {
            return Regex.IsMatch(text, regex);
        }

        /// <summary>
        /// Tests if a string is contained in an array of elements, separated by a semicolon like a CSV line.
        /// </summary>
        /// <param name="toSearch">String to search</param>
        /// <param name="csvSepString">String where to search</param>
        /// <param name="sepChar">The separator. Semicolon by default</param>
        /// <returns></returns>
        public static bool CsvStringContains(string toSearch, string csvSepString, char sepChar = ';')
        {
            string[] splitted = csvSepString.Split(sepChar);
            return splitted.Any(r => r.Equals(toSearch));
        }

        /// <summary>
        /// Determines whether a string contains all of the specified substrings using the "AND" operator.
        /// </summary>
        /// <param name="haystack">The string to search.</param>
        /// <param name="toSearch">An array of substrings to search for.</param>
        /// <returns>true if the string contains all of the specified substrings; otherwise, false.</returns>
        /// <remarks>
        /// This method determines whether the specified string contains all of the specified substrings using the "AND" operator.
        /// If the haystack string is null, the method returns false.
        /// If the toSearch array is null or empty, the method returns false.
        /// </remarks>
        public static bool ContainsMultipleWithAnd(string haystack, string[] toSearch)
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
        public static bool ContainsMultipleWithOr(string haystack, string[] toSearch)
        {
            if (haystack == null) return false;
            if (toSearch == null || !toSearch.Any()) return false;

            return toSearch.Any(haystack.Contains);
        }


        /// <summary>
        /// Return a string with random char and with a defined lenght. Charset is customizable.
        /// </summary>
        /// <param name="length">Lenght of the string</param>
        /// <param name="charSet">The charset</param>
        /// <param name="ensureUnique">If true, ensures that the generated string has not already been generated previously. Use the ClearRandomStringCache() method to clean the string cache already generated.</param>
        /// <returns></returns>
        public static string RandomString(int length, string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", bool ensureUnique = false)
        {

            Random random = new Random();
            string retString = new string(Enumerable.Repeat(charSet, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            if (ensureUnique)
            {
                if (_listRandomString == null)
                {
                    _listRandomString = new List<string>(1);
                }
                while (_listRandomString.Contains(retString))
                {
                    retString = RandomString(length, charSet, false);
                }
                _listRandomString.Add(retString);
            }

            return retString;
        }

        /// <summary>
        /// Cleans the cache of strings generated with the RandomString() method.
        /// </summary>
        public static void ClearRandomStringCache()
        {
            _listRandomString = new List<string>();
        }


        /// <summary>
        /// Converts an integer into its textual form. The output is only in France-French.
        /// </summary>
        /// <param name="n">the integer to convert to</param>
        /// <returns>The textual form of integer. France-French only</returns>
        public static string NumberToText(int n)
        {
            return RecNumberToText(n).Trim();
        }

        private static string RecNumberToText(int n)
        {
            if (n < 0)
                return "Moins " + RecNumberToText(-n);
            if (n == 0)
                return "";
            if (n <= 19)
                return new[] {"Un", "Deux", "Trois", "Quatre", "Cinq", "Six", "Sept", "Huit",
                    "Neuf", "Dix", "Onze", "Douze", "Treize", "Quartoze", "Quinze", "Seize",
                    "Dix-sept", "Dix-huit", "Dix-neuf"}[n - 1] + " ";

            if (n <= 99)
            {

                int r = n % 10;
                string sp = " ";
                if ((n >= 70 && n <= 79) || (n >= 90 && n <= 99))
                {
                    r = n % 20;
                    sp = string.Empty;
                }

                return new[]
                {
                    "Vingt", "Trente", "Quarante", "Cinquante", "Soixante", "Soixante-",
                    "Quatre-vingt", "Quatre-vingt-"
                }[n / 10 - 2] + sp + RecNumberToText(r);
            }

            if (n <= 199)
                return "Cent " + RecNumberToText(n % 100);
            if (n <= 999)
            {
                int div = n / 100;
                int mod = n % 100;
                return RecNumberToText(div) + "Cent" + (div >= 1 && mod == 0 ? "s" : "") + " " +
                       RecNumberToText(mod);
            }
            if (n <= 1999)
                return "Mille " + RecNumberToText(n % 1000);
            if (n <= 999999)
                return RecNumberToText(n / 1000) + "Mille " + RecNumberToText(n % 1000);
            if (n <= 1999999)
                return "Un million " + RecNumberToText(n % 1000000);
            if (n <= 999999999)
                return RecNumberToText(n / 1000000) + "Million " + RecNumberToText(n % 1000000);
            if (n <= 1999999999)
                return "Un milliard " + RecNumberToText(n % 1000000000);
            return RecNumberToText(n / 1000000000) + "Milliard " + RecNumberToText(n % 1000000000);
        }

        public static string MultipleReplaces(string summary, params string[] args )
        {
            
            if (string.IsNullOrWhiteSpace(summary)) return summary;
            if (args.Length % 2 != 0)
            {
                throw new ArgumentException("Le paramètre args doit avoir un nombre pair d'éléments");
            }

            string pattern = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (i % 2 == 0)
                {
                    pattern = args[i];
                }
                else
                {
                    string toReplaceWith = args[i];

                    summary = summary.Replace(pattern, toReplaceWith);
                }

            }

            return summary;
        }

        /// <summary>
        /// Converts a PascalCase string to a snake_case string.
        /// </summary>
        /// <param name="input">The PascalCase string to convert.</param>
        /// <returns>A snake_case string.</returns>
        /// <remarks>
        /// This method converts a PascalCase string to a snake_case string by inserting an underscore before each uppercase letter (except the first one) and converting all letters to lowercase.
        /// If the input string is null or empty, the method returns the input string.
        /// </remarks>
        public static string PascalToSnake(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder result = new StringBuilder();
            result.Append(char.ToLower(input[0]));

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result.Append("_");
                    result.Append(char.ToLower(input[i]));
                }
                else
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }
    }
}
