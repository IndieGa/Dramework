using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core
{
    internal static class WebRequest
    {
        #region ================================ FIELDS

        private static readonly string LogPrefix = $"[{nameof(WebRequest)}]";

        #endregion

        #region ================================ METHODS

        public static async UniTask<Response> Send(RequestOptions options)
        {
            var retries = 0;
            do
            {
                var request = CreateRequest(options);
                var sendAsyncOp = request.SendWebRequestWithOptions(options);
                if (options.ProgressCallback == null)
                {
                    await sendAsyncOp;
                }
                else
                {
                    options.ProgressCallback(0);

                    while (sendAsyncOp.isDone == false)
                    {
                        options.ProgressCallback(sendAsyncOp.progress);
                        await UniTask.Yield();
                    }

                    options.ProgressCallback(1);
                }

                var response = request.GetResponse(options);
                if (response.IsResponseSuccess)
                {
                    return Complete(response);
                }

                if (options.IsAborted == false && retries < options.Retries && (options.RetryCallbackOnlyOnNetworkErrors == false || request.result == UnityWebRequest.Result.ConnectionError))
                {
                    await UniTask.Delay(options.RetryMillisecondsDelay);
                    retries++;
                    Log(options.EnableLog, $"RestClient - Retry Request. Url: {options.Endpoint}. Method: {options.Method}", LogType.Log);
                }
                else
                {
                    return Complete(response);
                }
            } while (retries <= options.Retries);

            return null;

            Response Complete(Response response)
            {
                var logType = LogType.Log;
                switch (response.Status.ResponseClass)
                {
                    case HttpResponseClass.Informational:
                    case HttpResponseClass.Redirection:
                        logType = LogType.Warning;
                        break;
                    case HttpResponseClass.ClientError:
                    case HttpResponseClass.ServerError:
                        logType = LogType.Error;
                        break;
                }
                Log(options.EnableLog, $"RestClient - Response. Code: {response.Status.Code}({response.Status.Name}). Message: {response.Status.Description}", logType);
                return response;
            }
        }

        public static async UniTask<Response<T>> Send<T>(RequestOptions options)
        {
            var retries = 0;
            do
            {
                var request = CreateRequest(options);
                var sendAsyncOp = request.SendWebRequestWithOptions(options);
                if (options.ProgressCallback == null)
                {
                    await sendAsyncOp;
                }
                else
                {
                    options.ProgressCallback(0);

                    while (sendAsyncOp.isDone == false)
                    {
                        options.ProgressCallback(sendAsyncOp.progress);
                        await UniTask.Yield();
                    }

                    options.ProgressCallback(1);
                }

                var response = request.GetResponse<T>(options);
                if (response.IsResponseSuccess)
                {
                    // Log(options.EnableLog, $"RestClient - Response. Url: {options.Uri}. Method: {options.Method}. Response: {response.Text}", LogType.Log);
                    return Complete(response);
                }

                if (options.IsAborted == false && retries < options.Retries && (options.RetryCallbackOnlyOnNetworkErrors == false || request.result == UnityWebRequest.Result.ConnectionError))
                {
                    await UniTask.Delay(options.RetryMillisecondsDelay);
                    retries++;
                    Log(options.EnableLog, $"RestClient - Retry Request. Url: {options.Endpoint}. Method: {options.Method}", LogType.Log);
                }
                else
                {
                    return Complete(response);
                }
            } while (retries <= options.Retries);

            return null;

            Response<T> Complete(Response<T> response)
            {
                var logType = LogType.Log;
                switch (response.Status.ResponseClass)
                {
                    case HttpResponseClass.Informational:
                    case HttpResponseClass.Redirection:
                        logType = LogType.Warning;
                        break;
                    case HttpResponseClass.UnknownCode:
                    case HttpResponseClass.ClientError:
                    case HttpResponseClass.ServerError:
                        logType = LogType.Error;
                        break;
                }
                Log(options.EnableLog, $"RestClient - Response. Code: {response.Status.Code}({response.Status.Name}). Message: {response.Status.Description}", logType);
                return response;
            }
        }

        private static UnityWebRequest CreateRequest(RequestOptions options)
        {
            var url = CreateUrl(options.Endpoint, options.Params);
            Log(options.EnableLog, $"RestClient - Request. Url: {url}. Method: {options.Method}. Body: {(options.Body != null ? JsonConvert.SerializeObject(options.Body) : "No Body")}", LogType.Log);
            if (options.FormData != null && options.Method == UnityWebRequest.kHttpVerbPOST)
                return UnityWebRequest.Post(url, options.FormData);

            return new UnityWebRequest(url, options.Method);
        }

        private static string CreateUrl(string url, Dictionary<string, string> queryParams)
        {
            if (RestClient.DefaultRequestParams.Any() == false && queryParams.Any() == false) return url;

            var keys = queryParams.Concat(RestClient.DefaultRequestParams
                    .Where(p => queryParams.Keys.Contains(p.Key) == false))
                .Select(p => $"{p.Key}={p.Value}")
                .ToArray();

            url += $"{(url.Contains("?") ? "&" : "?")}{string.Join("&", keys)}";
            return url;
        }

        private static void Log(bool debugEnabled, string message, LogType logType)
        {
            if (debugEnabled == false) return;

            switch (logType)
            {
                case LogType.Error:
                    ConsoleLogger.LogError(message, LogPrefix);
                    break;
                case LogType.Warning:
                    ConsoleLogger.LogWarning(message, LogPrefix);
                    break;
                case LogType.Log:
                    ConsoleLogger.Log(message, LogPrefix);
                    break;
            }
        }

        #endregion
    }
}