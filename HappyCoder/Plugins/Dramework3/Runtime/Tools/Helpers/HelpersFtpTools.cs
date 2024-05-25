using System.Collections.Generic;
using System.IO;
using System.Net;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class FtpTools
        {
            #region ================================ METHODS

            public static IEnumerable<string> GetFtpFiles(string url, string user, string password)
            {
                var request = GetRequest(url, user, password, WebRequestMethods.Ftp.ListDirectory);
                var files = new List<string>();
                using var response = (FtpWebResponse)request.GetResponse();
                using var responseStream = response.GetResponseStream();
                if (responseStream == null) return files;
                using var reader = new StreamReader(responseStream);
                while (reader.EndOfStream == false)
                {
                    files.Add(reader.ReadLine());
                }
                return files;
            }

            public static FtpWebRequest GetRequest(string url, string user, string password, string method)
            {
                var request = (FtpWebRequest)WebRequest.Create(url);
                request.Credentials = new NetworkCredential(user, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                request.Method = method;
                return request;
            }

            #endregion
        }

        #endregion
    }
}