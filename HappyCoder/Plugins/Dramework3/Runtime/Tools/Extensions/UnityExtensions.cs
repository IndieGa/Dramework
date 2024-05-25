using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class UnityExtensions
    {
        #region ================================ METHODS

        public static Vector2 FocusOn(this ScrollRect instance, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = instance.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            var result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            return result;
        }

        public static T RemoveClonePostfix<T>(this T obj) where T : Object
        {
            obj.name = obj.name.Replace("(Clone)", "");
            return obj;
        }

        public static void SetAnchoredPosition(this RectTransform obj, RectTransform parent, PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, eventData.position, null, out var result);
            result += parent.sizeDelta / 2;
            obj.anchoredPosition = result;
        }

        public static void SetAnchoredPosition(this RectTransform obj, RectTransform parent, Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPosition, null, out var result);
            result += parent.sizeDelta / 2;
            obj.anchoredPosition = result;
        }

        public static Object SetName(this Object obj, string name)
        {
            obj.name = name;
            return obj;
        }

        #endregion
    }
}