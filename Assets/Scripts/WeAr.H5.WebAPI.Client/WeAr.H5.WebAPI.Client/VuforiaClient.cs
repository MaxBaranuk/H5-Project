//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Text;
//using Newtonsoft.Json;

//namespace WeAr.H5.WebAPI.Client
//{
//    public class VuforiaClient
//    {
//        //Server Keys
//        private string accessKey = "[ server access key ]";
//        private string secretKey = "[ server secret key ]";

//        private string url = "https://vws.vuforia.com";
//        private string targetName = "[ target name ]";
//        private string imageLocation = "[ file system path ]";

//        //private const float pollingIntervalMinutes = 60; //poll at 1-hour interval

//        public string PostTarget()
//        {
//            string createdTargetId = "";

//            try
//            {
//                createdTargetId = PostTargetInner();
//            }
//            catch (Exception)
//            {
//                return null;
//            }

//            return string.Empty;
//        }

//        private string GetRequestBody(byte[] image)
//        {
//            var vuforiaRequest = new VuforiaRequest()
//            {
//                name = Guid.NewGuid().ToString(),
//                width = 320.0f,
//                image = image,
//                active_flag = 1,
//                application_metadata = null
//            };

//            return JsonConvert.SerializeObject(vuforiaRequest);
//        }

//        private string PostTargetInner(byte[] image)
//        {
//            var queryData = GetRequestBody(image);
//            var headers = new NameValueCollection();
//            headers.Add("Date", DateTime.Now.ToString());
//            headers.Add("Authorization", "VWS " + accessKey + ":" + );

//        }
//    }

//    public class VuforiaRequest
//    {
//        public string name { get; set; }
//        public float width { get; set; }
//        public byte[] image { get; set; }
//        public byte active_flag { get; set; }
//        public byte[] application_metadata { get; set; }
//    }
//}
