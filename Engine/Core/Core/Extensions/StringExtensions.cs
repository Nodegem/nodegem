using System.Globalization;

namespace Nodester.Graph.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TakeAfter(this string str, string toBeSearched)
        {
            return str.Substring(str.IndexOf(toBeSearched) + toBeSearched.Length);
        }

        public static string ToTitleCase(this string @string) =>
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(@string);
    }
}