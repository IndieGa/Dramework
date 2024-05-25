using System;

using Newtonsoft.Json;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses
{
    [Serializable]
    public class RefreshIdTokenResponseBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// The number of seconds in which the ID token expires.
        /// </summary>
        [JsonProperty]
        private string expires_in;

        /// <summary>
        /// The type of the refresh token, always "Bearer".
        /// </summary>
        [JsonProperty]
        private string token_type;

        /// <summary>
        /// The Firebase Auth refresh token provided in the request or a new refresh token.
        /// </summary>
        [JsonProperty]
        private string refresh_token;

        /// <summary>
        /// A Firebase Auth ID token.
        /// </summary>
        [JsonProperty]
        private string id_token;

        /// <summary>
        /// The uid corresponding to the provided ID token.
        /// </summary>
        [JsonProperty]
        private string user_id;

        /// <summary>
        /// Your Firebase project ID.
        /// </summary>
        [JsonProperty]
        private string project_id;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [JsonIgnore]
        public string ExpiresIn => expires_in;

        [JsonIgnore]
        public string IdToken => id_token;

        [JsonIgnore]
        public string ProjectId => project_id;

        [JsonIgnore]
        public string RefreshToken => refresh_token;

        [JsonIgnore]
        public string TokenType => token_type;

        [JsonIgnore]
        public string UserId => user_id;

        #endregion
    }
}