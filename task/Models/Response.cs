using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace task.Models
{
    public class Response
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }

    }
}