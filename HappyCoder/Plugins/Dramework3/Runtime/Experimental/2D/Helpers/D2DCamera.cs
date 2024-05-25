using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Experimental._2D.Helpers
{
    [RequireComponent(typeof(Camera))]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class D2DCamera : DBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Camera", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Camera:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private Camera _camera;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/ID", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("ID:")]
        [SerializeField] [ShowIf("@AnyCameraID == false")]
        private string _id;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/ID", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("ID:")]
        [SerializeField] [ShowIf("@AnyCameraID == true")] [ValueDropdown("CameraIDs")]
        private string _selectedID;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Camera Size", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Camera Size:")]
        [SerializeField]
        private float _cameraSize = 5;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Use Safe Area", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Use Safe Area:")]
        [SerializeField]
        private bool _useSafeArea;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Vector2 BottomCenter { get; private set; }
        public Vector2 BottomLeft { get; private set; }
        public Vector2 BottomRight { get; private set; }
        public Camera Camera => _camera;
        public float CameraSize => _cameraSize;
        public float Height { get; private set; }
        public float HeightInPixels => PixelsPerUnit * Height;
        public Vector2 MiddleCenter { get; private set; }
        public Vector2 MiddleLeft { get; private set; }
        public Vector2 MiddleRight { get; private set; }
        public float PixelsPerUnit => Screen.currentResolution.height / Height;
        public Vector2 TopCenter { get; private set; }
        public Vector2 TopLeft { get; private set; }
        public Vector2 TopRight { get; private set; }
        public float Width => Height * _camera.aspect;
        public float WidthInPixels => HeightInPixels * _camera.aspect;

        #endregion

        #region ================================ METHODS

        public void ComputeResolution()
        {
            if (_camera == null) _camera = GetComponent<Camera>();
            if (_camera == null) return;

            _camera.orthographic = true;

            if (Screen.orientation is ScreenOrientation.LandscapeLeft or ScreenOrientation.LandscapeRight)
                _camera.orthographicSize = 1f / _camera.aspect * _cameraSize;
            else
                _camera.orthographicSize = _cameraSize;

            var center = Vector2.zero;
            var min = Vector2.zero;
            var max = Vector2.zero;
            float halfWidth, halfHeight;
            Height = 2f * _camera.orthographicSize;

            if (_useSafeArea)
            {
                Height = Screen.safeArea.height / PixelsPerUnit;
                halfWidth = Height * (Screen.safeArea.width / Screen.safeArea.height) * 0.5f;
                halfHeight = Height * 0.5f;
                center = (new Vector2(Screen.currentResolution.width * 0.5f, Screen.currentResolution.height * 0.5f) - Screen.safeArea.center) / PixelsPerUnit * new Vector2(1, -1);
                min.x = center.x - halfWidth;
                min.y = center.y - halfHeight;
                max.x = center.x + halfWidth;
                max.y = center.y + halfHeight;
            }
            else
            {
                halfWidth = Height * _camera.aspect * 0.5f;
                halfHeight = Height * 0.5f;
                min.x = -halfWidth;
                min.y = -halfHeight;
                max.x = halfWidth;
                max.y = halfHeight;
            }

            BottomLeft = new Vector2(min.x, min.y);
            BottomCenter = new Vector2(center.x, min.y);
            BottomRight = new Vector2(max.x, min.y);
            MiddleLeft = new Vector2(min.x, center.y);
            MiddleCenter = center;
            MiddleRight = new Vector2(max.x, center.y);
            TopLeft = new Vector2(min.x, max.y);
            TopCenter = new Vector2(center.x, max.y);
            TopRight = new Vector2(max.x, max.y);

#if UNITY_EDITOR
            OnChange();
#endif
        }

        private void Awake()
        {
            ComputeResolution();
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        private readonly List<Action> _onChangeListeners = new List<Action>();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public string ID => CameraIDs.Count > 0 ? _selectedID : _id;
        private bool AnyCameraID => CameraIDs.Count > 0;
        private IReadOnlyList<string> CameraIDs => Tools.Helpers.Helpers.EditorTools.GetStaticFieldsValues<string>("CameraID");

        #endregion

        #region ================================ METHODS

        internal void AddOnChangeListener(Action listener)
        {
            if (_onChangeListeners.Contains(listener)) return;
            _onChangeListeners.Add(listener);
        }

        internal void RemoveOnChangeListener(Action listener)
        {
            if (_onChangeListeners.Contains(listener) == false) return;
            _onChangeListeners.Remove(listener);
        }

        private void OnChange()
        {
            foreach (var listener in _onChangeListeners)
                listener.Invoke();
        }

        protected void OnValidate()
        {
            ComputeResolution();
        }

        protected override void OnEditorInitialize()
        {
            ComputeResolution();
        }

        #endregion

#endif
    }
}