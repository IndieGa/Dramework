#if UNITY_EDITOR

using System;
using System.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Experimental._2D.Helpers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;
using IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Helpers;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    public partial class DCore
    {
        #region ================================ FIELDS

        private static Vector2 _lastResolution;
        private static Rect _lastSafeArea;
        private static bool _editorLock;

        #endregion

        #region ================================ METHODS

        private static void DetectScreenChange()
        {
            if (Application.isFocused == false
                || _editorLock
                || (Math.Abs(_lastResolution.x - Screen.currentResolution.width) < 1
                    && Math.Abs(_lastResolution.y - Screen.currentResolution.height) < 1
                    && _lastSafeArea == Screen.safeArea)) return;
            {
                _editorLock = true;
                _lastResolution.x = Screen.currentResolution.width;
                _lastResolution.y = Screen.currentResolution.height;
                _lastSafeArea = Screen.safeArea;
                OnScreenChange();
            }
        }

        private static void EditorUpdate()
        {
            DetectScreenChange();
        }

        private static async void OnScreenChange()
        {
            await Task.Delay(100);

            foreach (var safeArea in Helpers.EditorTools.GetAllComponentsFromOpenScenes<DSafeArea>(true))
                safeArea.ApplySafeArea();

            foreach (var d2DCamera in Helpers.EditorTools.GetAllComponentsFromOpenScenes<D2DCamera>(true))
                d2DCamera.ComputeResolution();

            _editorLock = false;
        }

        [InitializeOnLoadMethod]
        private static void StaticInitialize()
        {
            EditorApplication.update += EditorUpdate;
        }

        #endregion
    }
}

#endif