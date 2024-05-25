using System;

using Newtonsoft.Json;


#pragma warning disable 414

namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Requests
{
    [Serializable]
    public class RefreshIdTokenRequestBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// The refresh token's grant type, always "refresh_token".
        /// </summary>
        [JsonProperty]
        private string grant_type;

        /// <summary>
        /// A Firebase Auth refresh token.
        /// </summary>
        [JsonProperty]
        private string refresh_token;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public RefreshIdTokenRequestBody(string refreshToken)
        {
            grant_type = "refresh_token";
            refresh_token = refreshToken;
        }

        #endregion
    }
}