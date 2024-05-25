using System.Collections.Generic;
using System.Linq;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class StringExtensions
    {
        #region ================================ METHODS

        public static int CharCount(this string source, char c)
        {
            return Helpers.Helpers.StringTools.CharCount(source, c);
        }

        public static string ClearText(this string text, string replace = "")
        {
            return Helpers.Helpers.StringTools.ClearText(text, replace);
        }

        public static string CreateUiElementName(this string text)
        {
            return Helpers.Helpers.UIToolKitTools.CreateUiElementName(text);
        }

        public static bool Equal(this string first, string second)
        {
            return first == second;
        }

        public static string FirstCharToUpper(this string text)
        {
            return Helpers.Helpers.StringTools.FirstCharToUpper(text);
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool NotAnyFrom(this string text, IEnumerable<string> collection)
        {
            return collection.All(value => text != value);
        }

        public static bool NotEqual(this string first, string second)
        {
            return first != second;
        }

        public static string[] SplitByUppercase(this string text)
        {
            return Helpers.Helpers.StringTools.SplitByUppercase(text);
        }

        public static int SubstringCount(this string source, string subString)
        {
            return Helpers.Helpers.StringTools.SubstringCount(source, subString);
        }

        #endregion
    }
}