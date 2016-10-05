﻿using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace WeAr.H5.WebAPI.Client
{
    public class WebApiClient
    {
        private static ApiResponse SendWithData<T>(EMethod method, string url, T queryData)
        {
            var encoding = new UTF8Encoding();
            var jsonData = JsonConvert.SerializeObject(queryData);
            var data = encoding.GetBytes(jsonData);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = method.ToString();
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = data.Length;

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            var location = response.GetResponseHeader("location");
            var strResult = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return new ApiResponse(response.StatusCode, strResult, location);
        }

        private static ApiResponse Send(EMethod method, string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method.ToString();
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            var location = response.GetResponseHeader("location");
            var strResult = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return new ApiResponse(response.StatusCode, strResult, location);
        }

        //public static void Send<TSend>(EMethod method, string url, TSend queryData = null) where TSend : class
        //{
        //    var result = queryData == null
        //                    ? Send(method, url)
        //                    : SendWithData(method, url, queryData);
        //}

        public static TReturn SendAndDeserialize<TSend, TReturn>(EMethod method, string url, TSend queryData)
        {
            var response = SendWithResponse(method, url, queryData);

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
    }
}