using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace WeAr.H5.WebAPI.Client
{
    public class WebApiClient
    {

        private static ApiResponse SendWithData<T>(EMethod method, string url, T queryData, NameValueCollection headers = null)
        {
            var encoding = new UTF8Encoding();
            var jsonData = JsonConvert.SerializeObject(queryData);
            var data = encoding.GetBytes(jsonData);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = method.ToString();
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = data.Length;

            if (headers != null)
            {
                httpWebRequest.Headers.Add(headers);
            }

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            string strResult = null;
            string location = null;
            HttpStatusCode statusCode;
            using (var response = (HttpWebResponse) httpWebRequest.GetResponse())
            {
                statusCode = response.StatusCode;
                location = response.GetResponseHeader("location");
                strResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
                
            return new ApiResponse(statusCode, strResult, location);
        }

        private static ApiResponse Send(EMethod method, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method.ToString();
            string strResult = null;
            string location = null;
            HttpStatusCode statusCode;
            using (var response = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                statusCode = response.StatusCode;
                location = response.GetResponseHeader("location");
                strResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return new ApiResponse(statusCode, strResult, location);
        }

        //public static void Send<TSend>(EMethod method, string url, TSend queryData = null) where TSend : class
        //{
        //    var result = queryData == null
        //                    ? Send(method, url)
        //                    : SendWithData(method, url, queryData);
        //}

        public static TReturn SendAndDeserialize<TSend, TReturn>(EMethod method, string url, TSend queryData)
        {
            var response =
                SendWithResponse(method, url, queryData);

            return response.StatusCode != HttpStatusCode.BadRequest
                ? JsonConvert.DeserializeObject<TReturn>(response.Result)
                : default(TReturn);
        }

        public static TReturn SendAndDeserialize<TReturn>(EMethod method, string url)
        {
            var response = Send(method, url);

            return response.StatusCode != HttpStatusCode.BadRequest
                ? JsonConvert.DeserializeObject<TReturn>(response.Result)
                : default(TReturn);
        }

        public static ApiResponse SendWithResponse<TSend>(EMethod method, string url, TSend queryData)
        {
            var response = SendWithData(method, url, queryData);

            return response;
        }

        public static ApiResponse SendWithResponse(EMethod method, string url)
        {
            var response = Send(method, url);

            return response;
        }

        private static string GetUrlQuery(object obj)
        {
            var properties = obj.GetType().GetProperties();

            var builder = new StringBuilder("?");

            for (int i = 0; i < properties.Length; i++)
            {
                builder.Append(properties);
            }

            return null;
        }
    }
}