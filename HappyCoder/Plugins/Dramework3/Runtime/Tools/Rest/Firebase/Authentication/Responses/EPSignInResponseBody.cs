using System;

using Newtonsoft.Json;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses
{
    /// <summary>
    /// Email / Password Sign In Response Body
    /// </summary>
    [Serializable]
    public class EPSignInResponseBody : EPSignUpResponseBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// Whether the email is for an existing account.
        /// </summary>
        [JsonProperty]
        private bool registered;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [JsonIgnore]
        public bool Registered => registered;

        #endregion
    }
}