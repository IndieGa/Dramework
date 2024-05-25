using System.Diagnostics.CodeAnalysis;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Requests;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Authentication.Responses;
using IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.RealtimeDatabase;

using Newtonsoft.Json;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Firebase.Core
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class FirebaseRestApi
    {
        #region ================================ FIELDS

        private const string PREFIX = "FirebaseRestApi";

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static string FirebaseUrl => DProjectConfig.FirebaseUrl;

        #endregion

        #region ================================ METHODS

        public static string GetEndPoint(string url)
        {
            return $"{DProjectConfig.FirebaseUrl}/{url}.json";
        }

        public static bool HasError(Response response)
        {
            switch (response.Status.Code)
            {
                case 400:
                    ConsoleLogger.LogError("One of the following error conditions:" +
                                           "\nUnable to parse PUT or POST data." +
                                           "\nMissing PUT or POST data." +
                                           "\nThe request attempts to PUT or POST data that is too large." +
                                           "\nThe REST API call contains invalid child names as part of the path." +
                                           "\nThe REST API call path is too long." +
                                           "\nThe request contains an unrecognized server value." +
                                           "\nThe index for the query is not defined in your Firebase Realtime Database Security Rules." +
                                           "\nThe request does not support one of the query parameters that is specified." +
                                           "\nThe request mixes query parameters with a shallow GET request.", $"[{PREFIX}.CheckErrors()]");
                    return true;
                case 401:
                    ConsoleLogger.LogError("One of the following error conditions:" +
                                           "\nThe auth token has expired." +
                                           "\nThe auth token used in the request is invalid." +
                                           "\nAuthenticating with an access_token failed." +
                                           "\nThe request violates your Firebase Realtime Database Security Rules.", $"[{PREFIX}.CheckErrors()]");
                    return true;
                case 404:
                    ConsoleLogger.LogError("The specified Firebase database was not found.", $"[{PREFIX}.CheckErrors()]");
                    return true;
                case 500:
                    ConsoleLogger.LogError("The server returned an error. See the error message for further details.", $"[{PREFIX}.CheckErrors()]");
                    return true;
                case 503:
                    ConsoleLogger.LogError("The specified Firebase Realtime Database is temporarily unavailable, which means the request was not attempted.", $"[{PREFIX}.CheckErrors()]");
                    return true;
            }
            return false;
        }

        private static bool CheckAppKey()
        {
            if (string.IsNullOrEmpty(DProjectConfig.FirebaseAppKey) == false) return true;
            ConsoleLogger.LogError("Google App Key is null or empty. Check project settings config in \"Plugins\\Dramework 3\\Resources\\Project Settings\"", PREFIX, DProjectConfig.Instance);
            return false;
        }

        #endregion

        #region ================================ NESTED TYPES

        public static class Authentication
        {
            #region ================================ METHODS

            public static async UniTask<Response<RefreshIdTokenResponseBody>> RefreshIdToken(string appKey, string refreshToken, bool enableDebug = false)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = $"{Constants.REFRESH_ID_TOKEN_ENDPOINT}{appKey}",
                    Body = new RefreshIdTokenRequestBody(refreshToken),
                    EnableLog = enableDebug
                };

                return await RefreshIdToken(requestOptions);
            }

            public static async UniTask<Response<RefreshIdTokenResponseBody>> RefreshIdToken(string refreshToken, bool enableDebug = false)
            {
                if (CheckAppKey() == false) return null;
                return await RefreshIdToken(DProjectConfig.FirebaseAppKey, refreshToken, enableDebug);
            }

            public static async UniTask<Response<EPSignInResponseBody>> SignIn(string appKey, string email, string password, bool enableDebug = false)
            {
                var body = new EPAuthRequestBody(email, password);
                var requestOptions = new RequestOptions
                {
                    Endpoint = $"{Constants.SIGNIN_ENDPOINT}{appKey}",
                    Body = body,
                    EnableLog = enableDebug
                };

                return await SignIn(requestOptions);
            }

            public static async UniTask<Response<EPSignInResponseBody>> SignIn(string email, string password, bool enableDebug = false)
            {
                if (CheckAppKey() == false) return null;
                return await SignIn(DProjectConfig.FirebaseAppKey, email, password, enableDebug);
            }

            public static async UniTask<Response<EPSignUpResponseBody>> SignUp(string appKey, string email, string password, bool enableDebug = false)
            {
                var body = new EPAuthRequestBody(email, password);
                var requestOptions = new RequestOptions
                {
                    Endpoint = $"{Constants.SIGNUP_ENDPOINT}{appKey}",
                    Body = body,
                    EnableLog = enableDebug
                };

                return await SignUp(requestOptions);
            }

            public static async UniTask<Response<EPSignUpResponseBody>> SignUp(string email, string password, bool enableDebug = false)
            {
                if (CheckAppKey() == false) return null;
                return await SignUp(DProjectConfig.FirebaseAppKey, email, password, enableDebug);
            }

            private static void LogError(string response, string method)
            {
                var error = JsonConvert.DeserializeObject<AuthErrorResponseBody>(response);
                if (error is not { error: not null } || string.IsNullOrEmpty(error.error.message)) return;
                switch (error.error.message)
                {
                    case "EMAIL_EXISTS":
                        ConsoleLogger.LogError("The email address is already in use by another account.", $"[{PREFIX}.{method}()]");
                        break;
                    case "OPERATION_NOT_ALLOWED":
                        ConsoleLogger.LogError("Password sign-in is disabled for this project.", $"[{PREFIX}.{method}()]");
                        break;
                    case "TOO_MANY_ATTEMPTS_TRY_LATER":
                        ConsoleLogger.LogError("We have blocked all requests from this device due to unusual activity. Try again later.", $"[{PREFIX}.{method}()]");
                        break;
                    case "EMAIL_NOT_FOUND":
                        ConsoleLogger.LogError("There is no user record corresponding to this identifier. The user may have been deleted.", $"[{PREFIX}.{method}()]");
                        break;
                    case "INVALID_PASSWORD":
                        ConsoleLogger.LogError("The password is invalid or the user does not have a password.", $"[{PREFIX}.{method}()]");
                        break;
                    case "USER_DISABLED":
                        ConsoleLogger.LogError("The user account has been disabled by an administrator.", $"[{PREFIX}.{method}()]");
                        break;
                    case "TOKEN_EXPIRED":
                        ConsoleLogger.LogError("The user's credential is no longer valid. The user must sign in again.", $"[{PREFIX}.{method}()]");
                        break;
                    case "USER_NOT_FOUND":
                        ConsoleLogger.LogError("The user corresponding to the refresh token was not found. It is likely the user was deleted.", $"[{PREFIX}.{method}()]");
                        break;
                    case "API key not valid. Please pass a valid API key. (invalid API key provided)":
                        ConsoleLogger.LogError("API key not valid. Please pass a valid API key. (invalid API key provided)", $"[{PREFIX}.{method}()]");
                        break;
                    case "INVALID_REFRESH_TOKEN":
                        ConsoleLogger.LogError("An invalid refresh token is provided.", $"[{PREFIX}.{method}()]");
                        break;
                    case "Invalid JSON payload received. Unknown name \"refresh_tokens\"":
                        ConsoleLogger.LogError("Cannot bind query parameter. Field 'refresh_tokens' could not be found in request message.", $"[{PREFIX}.{method}()]");
                        break;
                    case "INVALID_GRANT_TYPE":
                        ConsoleLogger.LogError("The grant type specified is invalid.", $"[{PREFIX}.{method}()]");
                        break;
                    case "MISSING_REFRESH_TOKEN":
                        ConsoleLogger.LogError("No refresh token provided.", $"[{PREFIX}.{method}()]");
                        break;
                    case "MISSING_GRANT_TYPE":
                        ConsoleLogger.LogError("MISSING_GRANT_TYPE.", $"[{PREFIX}.{method}()]");
                        break;
                    default:
                        ConsoleLogger.LogError(error.error.message, $"[{PREFIX}.{method}()]");
                        break;
                }
            }

            private static async UniTask<Response<RefreshIdTokenResponseBody>> RefreshIdToken(RequestOptions requestOptions)
            {
                const string methodName = nameof(RefreshIdToken);
                if (requestOptions.EnableLog)
                    ConsoleLogger.Log("Refresh id token is started.", $"[{PREFIX}.{methodName}()]");
                var response = await RestClient.Post<RefreshIdTokenResponseBody>(requestOptions);
                if (HasError(response))
                {
                    LogError(response.Text, methodName);
                }
                else
                {
                    if (requestOptions.EnableLog)
                        ConsoleLogger.Log("Refresh id token is completed.", $"[{PREFIX}.{methodName}()]");
                }

                return response;
            }

            private static async UniTask<Response<EPSignInResponseBody>> SignIn(RequestOptions requestOptions)
            {
                const string methodName = nameof(SignIn);

                if (((EPAuthRequestBody)requestOptions.Body).email.Contains('@') == false)
                {
                    ConsoleLogger.LogError("Email is incorrect", $"[{PREFIX}.{methodName}()]");
                    return Response<EPSignInResponseBody>.Empty;
                }

                if (((EPAuthRequestBody)requestOptions.Body).password.Length < 6)
                {
                    ConsoleLogger.LogError("Password must be at least 6 characters long", $"[{PREFIX}.{methodName}()]");
                    return Response<EPSignInResponseBody>.Empty;
                }

                if (requestOptions.EnableLog)
                    ConsoleLogger.Log($"Sign-In {((EPAuthRequestBody)requestOptions.Body).email} is started.", $"[{PREFIX}.{methodName}()]");
                var response = await RestClient.Post<EPSignInResponseBody>(requestOptions);
                if (HasError(response))
                {
                    LogError(response.Text, nameof(SignIn));
                }
                else
                {
                    if (requestOptions.EnableLog)
                        ConsoleLogger.Log($"Sign-In {response.Body.Email} is completed.", $"[{PREFIX}.{methodName}()]");
                }
                return response;
            }

            private static async UniTask<Response<EPSignUpResponseBody>> SignUp(RequestOptions requestOptions)
            {
                const string methodName = nameof(SignUp);
                var email = ((EPAuthRequestBody)requestOptions.Body).email;
                if (email.Contains('@') == false)
                {
                    ConsoleLogger.LogError("Email is incorrect", $"[{PREFIX}.{methodName}()]");
                    return Response<EPSignUpResponseBody>.Empty;
                }

                if (((EPAuthRequestBody)requestOptions.Body).password.Length < 6)
                {
                    ConsoleLogger.LogError("Password must be at least 6 characters long", $"[{PREFIX}.{methodName}()]");
                    return Response<EPSignUpResponseBody>.Empty;
                }

                if (requestOptions.EnableLog)
                    ConsoleLogger.Log("Sign-Up is started.", $"[{PREFIX}.{methodName}()]");
                var response = await RestClient.Post<EPSignUpResponseBody>(requestOptions);
                if (HasError(response))
                {
                    LogError(response.Text, nameof(SignUp));
                }
                else
                {
                    if (requestOptions.EnableLog)
                        ConsoleLogger.Log($"Sign-Up is completed. The account {email} is successfully created.", $"[{PREFIX}.{methodName}()]");
                }

                return response;
            }

            #endregion
        }

        public static class RealtimeDatabase
        {
            #region ================================ METHODS

            /// <summary>
            /// Remove data from the specified Firebase database reference.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Delete(string url, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Delete(requestOptions);
            }

            /// <summary>
            /// Remove data from the specified Firebase database reference.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Delete(RequestOptions options)
            {
                var response = await RestClient.Delete(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Delete));
                return response;
            }

            /// <summary>
            /// Read data from Firebase database by issuing a GET request to its URL endpoint
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response<T>> Get<T>(string url, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Get<T>(requestOptions);
            }

            /// <summary>
            /// Read data from Firebase database by issuing a GET request to its URL endpoint
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response<T>> Get<T>(RequestOptions options)
            {
                var response = await RestClient.Get<T>(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Get));
                return response;
            }

            /// <summary>
            /// Read data from Firebase database by issuing a GET request to its URL endpoint
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Get(string url, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Get(requestOptions);
            }

            /// <summary>
            /// Read data from Firebase database by issuing a GET request to its URL endpoint
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Get(RequestOptions options)
            {
                var response = await RestClient.Get(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Get));
                return response;
            }

            /// <summary>
            /// Update some of the keys for a defined path without replacing all of the data.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Patch(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Patch(requestOptions);
            }

            /// <summary>
            /// Update some of the keys for a defined path without replacing all of the data.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response> Patch(RequestOptions options)
            {
                var response = await RestClient.Patch(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Patch));
                return response;
            }

            /// <summary>
            /// Update some of the keys for a defined path without replacing all of the data.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response<T>> Patch<T>(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Patch<T>(requestOptions);
            }

            /// <summary>
            /// Update some of the keys for a defined path without replacing all of the data.
            /// </summary>
            /// <returns></returns>
            public static async UniTask<Response<T>> Patch<T>(RequestOptions options)
            {
                var response = await RestClient.Patch<T>(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Patch));
                return response;
            }

            /// <summary>
            /// Add to a list of data in our Firebase database. Every time we send a POST request, the Firebase client generates a unique key, like fireblog/users/<unique-id>/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response> Post(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Post(requestOptions);
            }

            /// <summary>
            /// Add to a list of data in our Firebase database. Every time we send a POST request, the Firebase client generates a unique key, like fireblog/users/<unique-id>/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response> Post(RequestOptions options)
            {
                var response = await RestClient.Post(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Post));
                return response;
            }

            /// <summary>
            /// Add to a list of data in our Firebase database. Every time we send a POST request, the Firebase client generates a unique key, like fireblog/users/<unique-id>/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response<T>> Post<T>(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Post<T>(requestOptions);
            }

            /// <summary>
            /// Add to a list of data in our Firebase database. Every time we send a POST request, the Firebase client generates a unique key, like fireblog/users/<unique-id>/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response<T>> Post<T>(RequestOptions options)
            {
                var response = await RestClient.Post<T>(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Post));
                return response;
            }

            /// <summary>
            /// Write or replace data to a defined path, like fireblog/users/user1/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response> Put(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Put(requestOptions);
            }

            /// <summary>
            /// Write or replace data to a defined path, like fireblog/users/user1/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response> Put(RequestOptions options)
            {
                var response = await RestClient.Put(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Put));
                return response;
            }

            /// <summary>
            /// Write or replace data to a defined path, like fireblog/users/user1/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response<T>> Put<T>(string url, object data, bool enableDebug = false, bool forceErrors = true)
            {
                var requestOptions = new RequestOptions
                {
                    Endpoint = url,
                    Body = data,
                    EnableLog = enableDebug,
                    ForceErrors = forceErrors
                };
                return await Put<T>(requestOptions);
            }

            /// <summary>
            /// Write or replace data to a defined path, like fireblog/users/user1/<data>
            /// </summary>
            /// <returns></returns>
            [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
            public static async UniTask<Response<T>> Put<T>(RequestOptions options)
            {
                var response = await RestClient.Put<T>(options);
                if ((options.ForceErrors || options.EnableLog) && HasError(response))
                    LogError(response.Text, nameof(Put));
                return response;
            }

            private static void LogError(string response, string method)
            {
                var error = JsonConvert.DeserializeObject<RDError>(response);
                ConsoleLogger.LogError(error.error, $"[{PREFIX}.{method}()]");
            }

            #endregion
        }

        #endregion
    }
}