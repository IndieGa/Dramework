using IG.HappyCoder.Dramework3.Runtime.Tools.Extensions;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class UIToolKitTools
        {
            #region ================================ METHODS

            public static string CreateUiElementName(string text)
            {
                var elementName = text.ClearText().FirstCharToUpper();
                var nameParts = elementName.SplitByUppercase();
                elementName = $"{nameParts[0]} -";
                for (var i = 1; i < nameParts.Length; i++)
                    elementName = $"{elementName} {nameParts[i]}";
                return elementName;
            }

            #endregion
        }

        #endregion
    }
}