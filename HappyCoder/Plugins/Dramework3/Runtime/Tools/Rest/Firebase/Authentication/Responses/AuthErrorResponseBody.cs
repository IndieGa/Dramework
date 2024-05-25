using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses
{
    /// <summary>
    /// Authentication Error Response Body
    /// </summary>
    [Serializable]
    public class AuthErrorResponseBody
    {
        #region ================================ FIELDS

        public Error error;

        #endregion

        #region ================================ NESTED TYPES

        [Serializable]
        public class Error
        {
            #region ================================ FIELDS

            public int code;
            public string message;
            public Item[] errors;

            #endregion

            #region ================================ NESTED TYPES

            [Serializable]
            public class Item
            {
                #region ================================ FIELDS

                public string message;
                public string domain;
                public string reason;

                #endregion
            }

            #endregion
        }

        #endregion
    }
}