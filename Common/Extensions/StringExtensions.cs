using System;
using System.Globalization;
using System.Linq;

namespace Nodegem.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateAtWord(this string input, int length)
        {
            if (input == null || input.Length < length)
                return input;

            var iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);

            return $"{input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()}...";
        }
        
        public static string SplitOnCapitalLetters(this string str)
        {
            return str.Aggregate(string.Empty, (result, next) =>
            {
                if (char.IsUpper(next) && result.Length > 0)
                {
                    result += ' ';
                }
                return result + next;
            });
        }
        
        public static string TakeAfter(this string str, string toBeSearched)
        {
            return str.Substring(str.IndexOf(toBeSearched, StringComparison.Ordinal) + toBeSearched.Length);
        }

        public static string ToTitleCase(this string @string) =>
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(@string);
    }
}