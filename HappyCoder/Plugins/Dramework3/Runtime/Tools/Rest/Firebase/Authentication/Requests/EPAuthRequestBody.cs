using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Requests
{
    /// <summary>
    /// Email / Password Authentication Request Body
    /// </summary>
    [Serializable]
    public class EPAuthRequestBody
    {
        #region ================================ FIELDS

        /// <summary>
        /// The email for the user to create or the email the user is signing in with..
        /// </summary>
        public string email;

        /// <summary>
        /// The password for the user to create or the password for the account..
        /// </summary>
        public string password;

        /// <summary>
        /// Whether or not to return an ID and refresh token. Should always be true.
        /// </summary>
        public bool returnSecureToken;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public EPAuthRequestBody(string email, string password)
        {
            this.email = email;
            this.password = password;
            returnSecureToken = true;
        }

        #endregion
    }
}