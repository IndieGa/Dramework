using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [HideMonoScript]
    public class ProgressbarOnSlider : DBehaviour
    {
        #region ================================ FIELDS

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/COMPONENTS")] [BoxGroup("@Title/COMPONENTS/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField] [GetComponent]
        private Slider _slider;

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/COMPONENTS")] [BoxGroup("@Title/COMPONENTS/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField] [GetComponent]
        private DLabel _labelCount;

        [FoldoutGroup("SETTINGS", 40)] [BoxGroup("SETTINGS/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField]
        private float _speed = 1;

        [FoldoutGroup("SETTINGS", 40)] [BoxGroup("SETTINGS/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField]
        private bool _stepLabelValue;

        [FoldoutGroup("SETTINGS", 40)] [BoxGroup("SETTINGS/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField]
        private float _labelCoeff = 1;

        [FoldoutGroup("RUNTIME", 70)] [BoxGroup("RUNTIME/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField]
        private float _value;

        [FoldoutGroup("RUNTIME", 70)] [BoxGroup("RUNTIME/BOX PROGRESSBAR ON SLIDER", false)]
        [SerializeField]
        private bool _hasLabel;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private float _delta;
        private int _sign;

        #endregion

        #region ================================ METHODS

        public void ResetValue()
        {
            _slider.normalizedValue = 0;
            _value = 0;
            if (_hasLabel)
                _labelCount.text = "0";
        }

        public void Set(float value)
        {
            _slider.normalizedValue = value;
            // _value = Mathf.Clamp01(value);
        }

        public void Set(int value)
        {
            _slider.normalizedValue = value;
            // _value = Mathf.Clamp01(value);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void Start()
        {
            _hasLabel = _labelCount != null;
            // _updateValue = StartCoroutine(UpdateValue());
        }

        [SuppressMessage("ReSharper", "FunctionNeverReturns")]
        private async UniTaskVoid UpdateValue()
        {
            while (true)
            {
                _delta = _value - _slider.normalizedValue;
                while (Mathf.Abs(_delta) > 0.01f)
                {
                    var sign = _delta >= 0 ? 1 : -1;
                    _slider.normalizedValue += sign * Time.deltaTime * _speed;
                    _delta = _value - _slider.normalizedValue;
                    if (_hasLabel)
                        _labelCount.text = _stepLabelValue ? ((int)(_slider.normalizedValue * _labelCoeff)).ToString() : (_slider.normalizedValue * _labelCoeff).ToString("0.0");
                    await UniTask.Yield(_cancellationTokenSource.Token);
                }
                await UniTask.Yield(_cancellationTokenSource.Token);
            }
        }

        #endregion
    }
}