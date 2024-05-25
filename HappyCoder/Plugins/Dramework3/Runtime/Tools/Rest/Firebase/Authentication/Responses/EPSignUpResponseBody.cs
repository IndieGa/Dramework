using System;

using Newtonsoft.Json;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses
{
    /// <summary>
    /// Email / Password Sign Up Response Body
    /// </summary>
    [Serializable]
    public class EPSignUpResponseBody : AuthResponseBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// The email for the authenticated user.
        /// </summary>
        [JsonProperty]
        private string email;

        /// <summary>
        /// he uid of the authenticated user.
        /// </summary>
        [JsonProperty]
        private string localId;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [JsonIgnore]
        public string Email => email;
        [JsonIgnore]
        public string LocalId => localId;

        #endregion
    }
}