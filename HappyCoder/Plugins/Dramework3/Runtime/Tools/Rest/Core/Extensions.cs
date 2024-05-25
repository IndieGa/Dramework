using System;
using System.Text;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core
{
    internal static class Extensions
    {
        #region ================================ METHODS

        public static HttpStatus GetStatus(this UnityWebRequest request)
        {
            return HttpHelper.GetHttpStatus(request.responseCode);
        }

        public static bool IsSuccess(this UnityWebRequest request, RequestOptions options)
        {
            var isNetworkError = request.result == UnityWebRequest.Result.ConnectionError;
            var isHttpError = request.result == UnityWebRequest.Result.ProtocolError;
            return request.isDone && isNetworkError == false && (isHttpError == false || options.IgnoreHttpException) && string.IsNullOrEmpty(request.error);
        }

        internal static Response GetResponse(this UnityWebRequest request, RequestOptions options)
        {
            return new Response(request, options);
        }

        internal static Response<T> GetResponse<T>(this UnityWebRequest request, RequestOptions options)
        {
            return new Response<T>(request, options);
        }

        internal static AsyncOperation SendWebRequestWithOptions(this UnityWebRequest request, RequestOptions options)
        {
            var bodyRaw = options.BodyRaw;

            if (options.Headers.TryGetValue(Constants.CONTENT_TYPE_HEADER, out var contentType) == false && options.UseDefaultContentType)
            {
                contentType = Constants.DEFAULT_CONTENT_TYPE;
            }

            if (options.Body != null || string.IsNullOrEmpty(options.BodyString) == false)
            {
                var bodyString = options.BodyString;
                if (options.Body != null)
                    bodyString = JsonConvert.SerializeObject(options.Body);

                bodyRaw = Encoding.UTF8.GetBytes(bodyString.ToCharArray());
            }
            else if (options.SimpleForm is { Count: > 0 })
            {
                bodyRaw = UnityWebRequest.SerializeSimpleForm(options.SimpleForm);
                contentType = "application/x-www-form-urlencoded";
            }
            else if (options.FormSections is { Count: > 0 })
            {
                contentType = GetFormSectionsContentType(out bodyRaw, options);
            }
            else if (options.FormData != null)
            {
                //The Content-Type header will be copied from the formData parameter
                contentType = string.Empty;
            }
            if (string.IsNullOrEmpty(options.ContentType) == false)
            {
                contentType = options.ContentType;
            }

            ConfigureWebRequestWithOptions(request, bodyRaw, contentType, options);
            return request.SendWebRequest();
        }

        private static void ConfigureWebRequestWithOptions(UnityWebRequest request, byte[] bodyRaw, string contentType, RequestOptions options)
        {
            if (options.CertificateHandler != null)
                request.certificateHandler = options.CertificateHandler;

            if (options.UploadHandler != null)
                request.uploadHandler = options.UploadHandler;

            if (bodyRaw != null)
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.uploadHandler.contentType = contentType;
            }

            request.downloadHandler = options.DownloadHandler ?? new DownloadHandlerBuffer();

            if (string.IsNullOrEmpty(contentType) == false)
                request.SetRequestHeader(Constants.CONTENT_TYPE_HEADER, contentType);

            foreach (var header in RestClient.DefaultRequestHeaders)
                request.SetRequestHeader(header.Key, header.Value);

            foreach (var header in options.Headers)
                request.SetRequestHeader(header.Key, header.Value);

            if (options.Timeout.HasValue)
                request.timeout = options.Timeout.Value;

            if (options.UseHttpContinue.HasValue)
                request.useHttpContinue = options.UseHttpContinue.Value;

            if (options.RedirectLimit.HasValue)
                request.redirectLimit = options.RedirectLimit.Value;

            options.Request = request;
        }

        private static string GetFormSectionsContentType(out byte[] bodyRaw, RequestOptions options)
        {
            var boundary = UnityWebRequest.GenerateBoundary();
            var formSections = UnityWebRequest.SerializeFormSections(options.FormSections, boundary);
            var terminate = Encoding.UTF8.GetBytes(string.Concat("\r\n--", Encoding.UTF8.GetString(boundary), "--"));
            bodyRaw = new byte[formSections.Length + terminate.Length];
            Buffer.BlockCopy(formSections, 0, bodyRaw, 0, formSections.Length);
            Buffer.BlockCopy(terminate, 0, bodyRaw, formSections.Length, terminate.Length);
            return string.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary));
        }

        #endregion
    }
}