using System;
using System.Collections.Specialized;
using System.Net;

namespace TeknoHook
{
    class Http
    {
        public static byte[] Post(string url, NameValueCollection pairs)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.UploadValues(url, pairs);
            }
        }
    }
}
