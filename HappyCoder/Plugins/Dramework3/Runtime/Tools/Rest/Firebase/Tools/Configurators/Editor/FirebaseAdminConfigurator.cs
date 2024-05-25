using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Tools.Configurators.Editor
{
    [HideMonoScript]
    [CreateAssetMenu(menuName = "Happy Coder/Dramework 3/Configs/Firebase/Admin", fileName = "Firebase Admin Config")]
    public sealed class FirebaseAdminConfigurator : DScriptableObjectWS
    {
        #region ================================ FIELDS

        [BoxGroup("Main", false)] [BoxGroup("Main/Fields", false)]
        [SerializeField] [LabelText("Email:")]
        public string _email;

        private readonly string Prefix = nameof(FirebaseAdminConfigurator);
        [BoxGroup("Main", false)] [BoxGroup("Main/Fields", false)]
        [SerializeField] [LabelText("Password:")]
        private string _password;
        [BoxGroup("Main", false)] [BoxGroup("Main/Fields", false)]
        [SerializeField] [LabelText("Min Password Length:")]
        private int _minPasswordLength;

        private bool _processing;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region ================================ METHODS

        private bool CheckRules()
        {
            if (_password.Length < _minPasswordLength)
            {
                ConsoleLogger.LogError($"Password must be at least {_minPasswordLength} characters long", $"[{Prefix}.CheckRules()]", this);
                return false;
            }

            return true;
        }

        [BoxGroup("Main", false)]
        [Button(ButtonSizes.Large)]
        private async void CreateAccount()
        {
            if (_processing || CheckRules() == false) return;

            TimeoutTimer().Forget();
            using var response = await FirebaseRestApi.Authentication.SignUp(_email, _password);
            _processing = false;
            _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid TimeoutTimer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _processing = true;
            await UniTask.Delay(10000, cancellationToken: _cancellationTokenSource.Token);
            _cancellationTokenSource = null;
            _processing = false;
        }

        #endregion
    }
}