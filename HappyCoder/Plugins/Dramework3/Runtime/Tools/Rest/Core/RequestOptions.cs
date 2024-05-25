using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine;
using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core
{
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class RequestOptions
    {
        #region ================================ FIELDS

        private const string LOG_PREFIX = "[RequestOptions]";

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        /// <summary>
        /// The data to send to the server, encoding the body with JsonUtility
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// The data as byte array to send to the server
        /// </summary>
        public byte[] BodyRaw { get; set; }

        /// <summary>
        /// The data serialized as string to send to the web server (Using other tools instead of JsonUtility)
        /// </summary>
        public string BodyString { get; set; }

        /// <summary>
        /// Holds a reference to a CertificateHandler object, which manages certificate validation for this UnityWebRequest.
        /// </summary>
        public CertificateHandler CertificateHandler { get; set; }

        /// <summary>
        /// Override the content type of the request manually
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Returns the number of bytes of body data the system has downloaded from the remote server. (Read Only)
        /// </summary>
        public ulong DownloadedBytes
        {
            get
            {
                ulong bytes = 0;
                if (Request != null)
                {
                    bytes = Request.downloadedBytes;
                }
                return bytes;
            }
        }

        /// <summary>
        /// Holds a reference to a DownloadHandler object, which manages body data received from the remote server by this UnityWebRequest.
        /// </summary>
        public DownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Returns a floating-point value between 0.0 and 1.0, indicating the progress of downloading body data from the server. (Read Only)
        /// </summary>
        public float DownloadProgress
        {
            get
            {
                float progress = 0;
                if (Request != null)
                {
                    progress = Request.downloadProgress;
                }
                return progress;
            }
        }

        /// <summary>
        /// Enable logs of the requests for debug mode
        /// </summary>
        public bool EnableLog { get; set; }

        /// <summary>
        /// Defines the target URL for the UnityWebRequest to communicate with
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Forced show error logs of the requests
        /// </summary>
        public bool ForceErrors { get; set; }

        /// <summary>
        /// The form data to send to the web server using WWWForm
        /// </summary>
        public WWWForm FormData { get; set; }

        /// <summary>
        /// The form data to send to the web server using IMultipartFormSection
        /// </summary>
        public List<IMultipartFormSection> FormSections { get; set; }

        /// <summary>
        /// The HTTP headers added manually to send with the request
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Prevent to catch http exceptions
        /// </summary>
        public bool IgnoreHttpException { get; set; }

        /// <summary>
        /// Check if the request was aborted
        /// </summary>
        /// <value>A boolean to know if the request was aborted by the user</value>
        public bool IsAborted { get; private set; }

        /// <summary>
        /// Defines the HTTP verb used by this UnityWebRequest, such as GET or POST.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The HTTP query string params to send with the request
        /// </summary>
        public Dictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// A callback executed everytime the requests progress changes (From 0 to 1)
        /// </summary>
        public Action<float> ProgressCallback { get; set; }

        /// <summary>
        /// Indicates the number of redirects which this UnityWebRequest will follow before halting with a “Redirect Limit Exceeded” system error.
        /// </summary>
        public int? RedirectLimit { get; set; }

        /// <summary>
        /// public use
        /// </summary>
        public UnityWebRequest Request { private get; set; }

        /// <summary>
        /// The number of retries of the request
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        /// Invoke RetryCallback only when the retry is provoked by a network error. (Default: true).
        /// </summary>
        public bool RetryCallbackOnlyOnNetworkErrors { get; set; }

        /// <summary>
        /// Milliseconds of delay to make a retry
        /// </summary>
        public int RetryMillisecondsDelay { get; set; }

        /// <summary>
        /// The form data to send to the web server using Dictionary
        /// </summary>
        public Dictionary<string, string> SimpleForm { get; set; }

        /// <summary>
        /// Sets UnityWebRequest to attempt to abort after the number of seconds in timeout have passed.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Returns the number of bytes of body data the system has uploaded to the remote server. (Read Only)
        /// </summary>
        public ulong UploadedBytes
        {
            get
            {
                ulong bytes = 0;
                if (Request != null)
                {
                    bytes = Request.uploadedBytes;
                }
                return bytes;
            }
        }

        /// <summary>
        /// Holds a reference to the UploadHandler object which manages body data to be uploaded to the remote server.
        /// </summary>
        public UploadHandler UploadHandler { get; set; }

        /// <summary>
        /// Returns a floating-point value between 0.0 and 1.0, indicating the progress of uploading body data to the server.
        /// </summary>
        public float UploadProgress
        {
            get
            {
                float progress = 0;
                if (Request != null)
                {
                    progress = Request.uploadProgress;
                }
                return progress;
            }
        }

        /// <summary>
        /// Enable or Disable Content Type JSON by default
        /// </summary>
        /// <value>Check if application/json is enabled by default</value>
        public bool UseDefaultContentType { get; set; }

        /// <summary>
        /// Determines whether this UnityWebRequest will include Expect: 100-Continue in its outgoing request headers. (Default: true).
        /// </summary>
        public bool? UseHttpContinue { get; set; }

        #endregion

        #region ================================ METHODS

        /// <summary>
        /// Abort the request manually
        /// </summary>
        public void Abort()
        {
            if (IsAborted || Request == null) return;

            try
            {
                IsAborted = true;
                if (Request.isDone == false)
                {
                    Request.Abort();
                }
            }
            catch (Exception error)
            {
                DebugLog(EnableLog, error.Message, true);
            }
            finally
            {
                Request = null;
            }
        }

        /// <summary>
        /// Get the value of a header
        /// </summary>
        /// <returns>The string value of the header.</returns>
        /// <param name="name">The name of the header.</param>
        public string GetHeader(string name)
        {
            string headerValue;
            if (Request != null)
            {
                headerValue = Request.GetRequestHeader(name);
            }
            else
            {
                Headers.TryGetValue(name, out headerValue);
            }
            return headerValue;
        }

        private void DebugLog(bool debugEnabled, string message, bool isError)
        {
            if (debugEnabled == false) return;

            if (isError)
                ConsoleLogger.LogError(message, LOG_PREFIX);
            else
                ConsoleLogger.Log(message, LOG_PREFIX);
        }

        #endregion
    }
}