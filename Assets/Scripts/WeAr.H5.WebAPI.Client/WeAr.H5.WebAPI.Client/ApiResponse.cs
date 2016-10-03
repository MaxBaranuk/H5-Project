using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WeAr.H5.WebAPI.Client
{
    public class ApiResponse
    {
        public ApiResponse(HttpStatusCode statusCode, string result, string location)
        {
            StatusCode = statusCode;
            Result = result;
            Location = location;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Result { get; set; }

        public string Location { get; set; }
    }
}
