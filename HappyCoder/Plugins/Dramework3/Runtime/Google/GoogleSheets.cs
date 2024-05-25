#if UNITY_EDITOR

using System;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;


namespace IG.HappyCoder.Dramework3.Runtime.Google
{
    public static class GoogleSheets
    {
        #region ================================ FIELDS

        private const string POSTFIX_URL = "/export?format=tsv&";

        #endregion

        #region ================================ METHODS

        public static async Task<GoogleSheetsTable> GetTable(string docId)
        {
            var url = $"{docId.Remove(docId.IndexOf("/edit?", StringComparison.Ordinal))}{POSTFIX_URL}";
            var result = await DownloadData(url);
            Debug.Log($"Received from google docs\n{result}");
            return new GoogleSheetsTable(result);
        }

        private static async Task<string> DownloadData(string url)
        {
            using var webRequest = UnityWebRequest.Get(url);
            var op = webRequest.SendWebRequest();
            while (op.isDone == false) await Task.Yield();
#pragma warning disable CS0618
            if (webRequest.isNetworkError) throw new Exception(webRequest.error);
#pragma warning restore CS0618
            return webRequest.downloadHandler.text;
        }

        #endregion
    }
}
#endif