using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UIElements;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UIToolkit.Base
{
    [RequireComponent(typeof(UIDocument))]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DScreen : DBehaviour
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Components", 20)]
        [SerializeField] [ReadOnly]
        private UIDocument _doc;

        [Indent]
        [FoldoutGroup("Settings", 20)]
        [SerializeField]
        private ScreenState _initialState;

        private Type _screenType;

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public event Action<Type> OnAfterHide;
        public event Func<Type, UniTask> OnAfterHideChain;

        public event Action<Type> OnAfterShow;
        public event Func<Type, UniTask> OnAfterShowChain;
        public event Action<Type> OnBeforeHide;
        public event Func<Type, UniTask> OnBeforeHideChain;
        public event Action<Type> OnBeforeShow;
        public event Func<Type, UniTask> OnBeforeShowChain;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Type ScreenType
        {
            get
            {
                if (_screenType != null)
                    return _screenType;
                return _screenType = GetType();
            }
        }
        public ScreenState State { get; private set; }

        #endregion

        #region ================================ METHODS

        public async UniTask HideAsync()
        {
            OnBeforeHide?.Invoke(ScreenType);
            if (OnBeforeHideChain != null)
                await OnBeforeHideChain.Invoke(ScreenType);

            _doc.rootVisualElement.style.display = DisplayStyle.None;
            State = ScreenState.Hidden;

            OnAfterHide?.Invoke(ScreenType);
            if (OnAfterHideChain != null)
                await OnAfterHideChain.Invoke(ScreenType);
        }

        public T Query<T>(string elementName = "") where T : VisualElement
        {
            return _doc.rootVisualElement.Q<T>(elementName);
        }

        public async UniTask ShowAsync(int sortingOrder = 0)
        {
            OnBeforeShow?.Invoke(ScreenType);
            if (OnBeforeShowChain != null)
                await OnBeforeShowChain.Invoke(ScreenType);

            _doc.rootVisualElement.style.display = DisplayStyle.Flex;
            _doc.sortingOrder = sortingOrder;
            State = ScreenState.Visible;

            OnAfterShow?.Invoke(ScreenType);
            if (OnAfterShowChain != null)
                await OnAfterShowChain.Invoke(ScreenType);
        }

        #endregion

        #region ================================ NESTED TYPES

        public enum ScreenState
        {
            Hidden,
            Visible
        }

        #endregion

#if UNITY_EDITOR

        private Dictionary<DScreen, ScreenState> _screens;

        [FoldoutGroup("Controls", 10000)] [HorizontalGroup("Controls/Buttons")]
        [Button]
        private async void Show()
        {
            await ShowAsync();
        }

        [FoldoutGroup("Controls", 10000)] [HorizontalGroup("Controls/Buttons")]
        [Button]
        private async void Hide()
        {
            await HideAsync();
        }

        [FoldoutGroup("Controls", 10000)] [HorizontalGroup("Controls/Buttons")]
        [Button]
        private async void ShowSingle()
        {
            await BackAsync();
            _screens = new Dictionary<DScreen, ScreenState>();
            var screens = Helpers.EditorTools.GetAllComponentsFromOpenScenes<DScreen>(false);
            foreach (var screen in screens)
            {
                _screens.Add(screen, screen.State);
                await screen.HideAsync();
            }
            await ShowAsync();
        }

        [FoldoutGroup("Controls", 10000)] [HorizontalGroup("Controls/Buttons")]
        [Button]
        private void Back()
        {
            BackAsync().Forget();
        }

        private async UniTask BackAsync()
        {
            if (_screens == null) return;

            foreach (var pair in _screens)
            {
                switch (pair.Value)
                {
                    case ScreenState.Hidden:
                        await pair.Key.HideAsync();
                        break;
                    case ScreenState.Visible:
                        await pair.Key.ShowAsync();
                        break;
                }
            }
        }

#endif
    }
}