using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JetDevsPrcatical.Data.Response
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = null)
        {
            Success = true;
            Message = message;
            Data = data;
        }
        public Response(string message, int statusCode)
        {
            Success = false;
            Message = message;
            StatusCode = (HttpStatusCode)statusCode;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Error> Errors { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
