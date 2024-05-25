namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core
{
    internal static class Constants
    {
        #region ================================ FIELDS

        internal const string SIGNUP_ENDPOINT = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
        internal const string SIGNIN_ENDPOINT = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=";
        internal const string REFRESH_ID_TOKEN_ENDPOINT = "https://securetoken.googleapis.com/v1/token?key=";

        #endregion
    }
}