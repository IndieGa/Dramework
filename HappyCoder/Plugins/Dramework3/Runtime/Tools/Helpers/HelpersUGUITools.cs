using UnityEngine;
using UnityEngine.EventSystems;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class UGUITools
        {
            #region ================================ METHODS

            public static Vector2 GetAnchoredPosition(RectTransform rectTransform, PointerEventData eventData)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, null, out var result);
                result += rectTransform.sizeDelta / 2;
                return result;
            }

            public static Vector2 GetAnchoredPosition(RectTransform rectTransform, Vector2 screenPosition)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, null, out var result);
                result += rectTransform.sizeDelta / 2;
                return result;
            }

            public static Vector2 GetAnchoredPosition(RectTransform rectTransform, Vector2 screenPosition, Camera camera)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPosition, camera, out var result);
                result += rectTransform.sizeDelta / 2;
                return result;
            }

            public static void SnapScrollRectTo(Transform scrollRect, RectTransform contentPanel, Vector3 targetPosition)
            {
                Canvas.ForceUpdateCanvases();

                contentPanel.anchoredPosition =
                    (Vector2)scrollRect.InverseTransformPoint(contentPanel.position)
                    - (Vector2)scrollRect.InverseTransformPoint(targetPosition);
            }

            #endregion
        }

        #endregion
    }
}