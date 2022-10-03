using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using task.Models;
using task.Services;

namespace task.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ProcessFile([FromBody] Request requestPayload) {

            FileReading service = new FileReading(requestPayload.filePath);
            Response response = service.processFile();
            return Request.CreateResponse(response.statusCode, response);
        }

        [Route("api/File/logs")]
        [HttpGet]
        public HttpResponseMessage GetLogs()
        {
            Logger logger = new Logger();
            List<string> logs = logger.getLogs();
            return Request.CreateResponse(HttpStatusCode.Accepted, logs);
        }
    }
}
