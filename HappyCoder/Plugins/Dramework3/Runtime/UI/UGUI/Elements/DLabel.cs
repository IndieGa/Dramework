using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base;

using Sirenix.OdinInspector;

using TMPro;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [HideMonoScript]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DLabel : DUIBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Label", false)]
        [SerializeField] [GetComponent]
        private TextMeshProUGUI _label;

        private string _text;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public Color color
        {
            get => _label.color;
            set => _label.color = value;
        }

        public bool PrintComplete { get; private set; }

        public string text
        {
            get => _label.text;
            set => _label.text = value;
        }

        #endregion

        #region ================================ METHODS

        public async UniTask Print(string value, float interval)
        {
            _cancellationTokenSource.Cancel();
            _text = value;
            var delay = (int)interval * 1000;
            var queue = new Queue<char>(_text);
            _label.text = string.Empty;
            PrintComplete = false;

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();

                if (c == '<')
                {
                    var word = "";
                    word += c;
                    for (var i = 0; i < 6; i++)
                        word += queue.Dequeue();

                    if (word == "<sprite")
                    {
                        while (c != '>')
                        {
                            c = queue.Dequeue();
                            word += c;
                        }
                        _label.text += word;
                    }
                    else
                    {
                        var temp = "";
                        while (queue.Count > 0)
                            temp += queue.Dequeue();

                        queue = new Queue<char>(word + temp);
                        c = queue.Dequeue();
                        _label.text += c;
                    }
                }
                else
                {
                    _label.text += c;
                }

                await UniTask.Delay(delay, DelayType.DeltaTime, PlayerLoopTiming.Update, _cancellationTokenSource.Token);
            }

            CompletePrint();
        }

        public void SkipPrint()
        {
            CompletePrint();
            _label.text = _text;
        }

        protected override void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void CompletePrint()
        {
            _cancellationTokenSource.Cancel();
            PrintComplete = true;
        }

        #endregion
    }
}