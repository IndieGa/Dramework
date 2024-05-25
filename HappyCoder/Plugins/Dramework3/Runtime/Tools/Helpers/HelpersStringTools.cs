using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ FIELDS

        public const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #endregion

        #region ================================ NESTED TYPES

        public static class StringTools
        {
            #region ================================ METHODS

            public static int CharCount(string source, char c)
            {
                return source.Count(ch => ch == c);
            }

            public static string ClearText(string text, string replace = "")
            {
                text = Regex.Replace(text, @"^[\d_]*", replace);
                var regex = new Regex(@"[\W]+");
                text = regex.Replace(text, replace);
                return text;
            }

            public static string FirstCharToUpper(string text)
            {
                switch (text)
                {
                    case null: throw new ArgumentNullException(nameof(text));
                    case "": throw new ArgumentException($"{nameof(text)} cannot be empty", nameof(text));
                    default: return text.First().ToString().ToUpper() + text.Substring(1);
                }
            }

            public static string GetRandomString(int length)
            {
                return new string(Enumerable.Repeat(CHARS, length)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }

            public static string InsertSymbolBetweenUppercaseWords(string text, string symbol)
            {
                var words = SplitByUppercase(text);
                return words.Aggregate(string.Empty, (current, word) =>
                {
                    if (word != words.Last())
                        return current + $"{word}{symbol}";

                    return current + word;
                });
            }

            public static string[] SplitByUppercase(string text)
            {
                var words = Regex.Split(text, @"(?<!^)(?=[A-Z])");
                var result = new List<string>();
                var word = string.Empty;
                for (var i = 0; i < words.Length; i++)
                {
                    if (words[i].All(char.IsUpper))
                    {
                        word += words[i];
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(word))
                        {
                            result.Add(words[i]);
                        }
                        else
                        {
                            result.Add(word);
                            result.Add(words[i]);
                            word = null;
                        }
                    }
                }

                if (string.IsNullOrEmpty(word) == false)
                    result.Add(word);

                return result.ToArray();
            }

            public static int StringToProduct(string input)
            {
                return input.Aggregate(0, (current, c) => current * c);
            }

            public static int StringToSum(string input)
            {
                return input.Aggregate(0, (current, c) => current + c);
            }

            public static int SubstringCount(string source, string subString)
            {
                var split = source.Split(new[] { subString }, StringSplitOptions.None);
                return split.Length - 1;
            }

            public static string UrlEncode(string url)
            {
                var strRdr = new StringReader(url);
                var strWtr = new StringWriter();
                var charValue = strRdr.Read();
                while (charValue != -1)
                {
                    if (charValue is >= 48 and <= 57 // 0-9
                        || charValue is >= 65 and <= 90 // A-Z
                        || charValue is >= 97 and <= 122) // a-z
                    {
                        strWtr.Write((char)charValue);
                    }
                    else if (charValue == 32) // Space
                    {
                        strWtr.Write("+");
                    }
                    else
                    {
                        strWtr.Write("%{0:x2}", charValue);
                    }
                    charValue = strRdr.Read();
                }
                return strWtr.ToString();
            }

            #endregion
        }

        #endregion
    }
}