using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Cysharp.Threading.Tasks;

using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public static class RestClient
    {
        #region ================================ FIELDS

        private static Version _version;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static Dictionary<string, string> DefaultRequestHeaders { get; set; } = new Dictionary<string, string>();

        public static Dictionary<string, string> DefaultRequestParams { get; set; } = new Dictionary<string, string>();

        public static Version Version
        {
            get
            {
                if (_version == null)
                {
                    _version = new Version("0.0.1");
                }
                return _version;
            }
        }

        #endregion

        #region ================================ METHODS

        public static void ClearDefaultHeaders()
        {
            DefaultRequestHeaders.Clear();
        }

        public static void ClearDefaultParams()
        {
            DefaultRequestParams.Clear();
        }

        public static async UniTask<Response> Delete(string url, bool enableLog = false)
        {
            return await Delete(new RequestOptions { Endpoint = url, EnableLog = enableLog });
        }

        public static async UniTask<Response> Delete(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbDELETE;
            return await WebRequest.Send(options);
        }

        public static async UniTask<Response> Get(string url, bool enableLog = false)
        {
            return await Get(new RequestOptions { Endpoint = url, EnableLog = enableLog });
        }

        public static async UniTask<Response> Get(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbGET;
            return await WebRequest.Send(options);
        }

        public static async UniTask<Response<T>> Get<T>(string url, bool enableLog = false)
        {
            return await Get<T>(new RequestOptions { Endpoint = url, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Get<T>(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbGET;
            return await WebRequest.Send<T>(options);
        }

        public static async UniTask<Response<T[]>> GetArray<T>(string url, bool enableLog = false)
        {
            return await GetArray<T>(new RequestOptions { Endpoint = url, EnableLog = enableLog });
        }

        public static async UniTask<Response<T[]>> GetArray<T>(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbGET;
            return await WebRequest.Send<T[]>(options);
        }

        public static async UniTask<Response> Head(string url, bool enableLog = false)
        {
            return await Head(new RequestOptions { Endpoint = url, EnableLog = enableLog });
        }

        public static async UniTask<Response> Head(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbHEAD;
            return await WebRequest.Send(options);
        }

        public static async UniTask<Response<T>> Patch<T>(string url, object body, bool enableLog = false)
        {
            return await Patch<T>(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Patch<T>(string url, string bodyString, bool enableLog = false)
        {
            return await Patch<T>(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Patch<T>(RequestOptions options)
        {
            options.Method = "PATCH";
            return await WebRequest.Send<T>(options);
        }

        public static async UniTask<Response> Patch(string url, object body, bool enableLog = false)
        {
            return await Patch(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response> Patch(string url, string bodyString, bool enableLog = false)
        {
            return await Patch(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response> Patch(RequestOptions options)
        {
            options.Method = "PATCH";
            return await WebRequest.Send(options);
        }

        public static async UniTask<Response<T>> Post<T>(string url, object body, bool enableLog = false)
        {
            return await Post<T>(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Post<T>(string url, string bodyString, bool enableLog = false)
        {
            return await Post<T>(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Post<T>(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbPOST;
            return await WebRequest.Send<T>(options);
        }

        public static async UniTask<Response> Post(string url, object body, bool enableLog = false)
        {
            return await Post(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response> Post(string url, string bodyString, bool enableLog = false)
        {
            return await Post(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response> Post(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbPOST;
            return await WebRequest.Send(options);
        }

        public static async UniTask<Response<T[]>> PostArray<T>(string url, object body, bool enableLog = false)
        {
            return await PostArray<T>(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response<T[]>> PostArray<T>(string url, string bodyString, bool enableLog = false)
        {
            return await PostArray<T>(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response<T[]>> PostArray<T>(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbPOST;
            return await WebRequest.Send<T[]>(options);
        }

        public static async UniTask<Response<T>> Put<T>(string url, object body, bool enableLog = false)
        {
            return await Put<T>(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Put<T>(string url, string bodyString, bool enableLog = false)
        {
            return await Put<T>(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response<T>> Put<T>(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbPUT;
            return await WebRequest.Send<T>(options);
        }

        public static async UniTask<Response> Put(string url, object body, bool enableLog = false)
        {
            return await Put(new RequestOptions { Endpoint = url, Body = body, EnableLog = enableLog });
        }

        public static async UniTask<Response> Put(string url, string bodyString, bool enableLog = false)
        {
            return await Put(new RequestOptions { Endpoint = url, BodyString = bodyString, EnableLog = enableLog });
        }

        public static async UniTask<Response> Put(RequestOptions options)
        {
            options.Method = UnityWebRequest.kHttpVerbPUT;
            return await WebRequest.Send(options);
        }

        #endregion
    }
}