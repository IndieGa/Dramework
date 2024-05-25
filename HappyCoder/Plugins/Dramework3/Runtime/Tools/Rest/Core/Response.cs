using System;
using System.Collections.Generic;
using System.Net;

using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Newtonsoft.Json;

using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Rest.Core
{
    [Serializable]
    public class Response : IDisposable
    {
        #region ================================ FIELDS

        private UnityWebRequest _request;
        private RequestOptions _options;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public Response(UnityWebRequest request, RequestOptions options)
        {
            _request = request;
            _options = options;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public static Response Empty => new Response(new UnityWebRequest(""), new RequestOptions());
        public byte[] Bytes => _request.downloadHandler.data;
        public Dictionary<string, string> Headers => _request.GetResponseHeaders();
        public bool IsRequestSuccess => _request.IsSuccess(_options);
        public bool IsResponseSuccess => IsRequestSuccess && Status.ResponseClass == HttpResponseClass.Successful;
        public string RequestError => _request.error;
        public UnityWebRequest.Result RequestResult => _request.result;
        public HttpStatus Status => _request.GetStatus();
        public string Text => _request.downloadHandler.text;

        #endregion

        #region ================================ METHODS

        public void Dispose()
        {
            _request.Dispose();
        }

        /// <summary>
        /// Get the value of a header
        /// </summary>
        /// <returns>The string value of the header.</returns>
        /// <param name="name">The name of the header.</param>
        public string GetHeader(string name)
        {
            return _request.GetResponseHeader(name);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }

    [Serializable]
    public class Response<T> : Response
    {
        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public Response(UnityWebRequest request, RequestOptions options) : base(request, options)
        {
            if (Status.Code == (long)HttpStatusCode.NoContent || IsResponseSuccess == false) return;
            Body = JsonConvert.DeserializeObject<T>(Text);
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public new static Response<T> Empty => new Response<T>(new UnityWebRequest(""), new RequestOptions());

        public T Body { get; set; }

        #endregion
    }
}