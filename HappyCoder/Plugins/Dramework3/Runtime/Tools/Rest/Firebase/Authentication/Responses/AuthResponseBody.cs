using System;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core;

using Newtonsoft.Json;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses
{
    /// <summary>
    /// Base Authentication Response Body
    /// </summary>
    [Serializable]
    public class AuthResponseBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// Kind of response.
        /// </summary>
        [JsonProperty] [SerializeField]
        private string kind;

        /// <summary>
        /// A Firebase Auth ID token for the authenticated user.
        /// </summary>
        [JsonProperty] [SerializeField]
        private string idToken;

        /// <summary>
        /// A Firebase Auth refresh token for the authenticated user.
        /// </summary>
        [JsonProperty] [SerializeField]
        private string refreshToken;

        /// <summary>
        /// The number of seconds in which the ID token expires.
        /// </summary>
        [JsonProperty] [SerializeField]
        private string expiresIn;

        [SerializeField] [HideInInspector]
        private long _receivedTime;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public AuthResponseBody()
        {
            InitExpire();
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [JsonIgnore]
        public double ExpiresIn { get; private set; }
        [JsonIgnore]
        public string IdToken => idToken;
        [JsonIgnore]
        public bool IsValid => string.IsNullOrEmpty(idToken) == false && string.IsNullOrEmpty(refreshToken) == false && string.IsNullOrEmpty(expiresIn) == false;
        [JsonIgnore]
        public string RefreshToken => refreshToken;

        #endregion

        #region ================================ METHODS

        public bool IsExpired()
        {
            return _receivedTime + ExpiresIn >= Helpers.Helpers.DateTimeTools.DateTimeToUnix(DateTime.Now);
        }

        public async UniTask Refresh()
        {
            if (IsExpired() == false) return;
            using var response = await FirebaseRestApi.Authentication.RefreshIdToken(refreshToken);
            if (response.IsResponseSuccess == false) return;
            idToken = response.Body.IdToken;
            refreshToken = response.Body.RefreshToken;
            expiresIn = response.Body.ExpiresIn;
            InitExpire();
        }

        private void InitExpire()
        {
            _receivedTime = Helpers.Helpers.DateTimeTools.DateTimeToUnix(DateTime.Now);
            if (double.TryParse(expiresIn, out var value))
                ExpiresIn = value;
        }

        #endregion
    }
}