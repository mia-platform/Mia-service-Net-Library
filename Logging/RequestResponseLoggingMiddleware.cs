using System.IO;
using System.Threading.Tasks;
using Logging.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            var incomingRequestLog = BuildIncomingRequestLog(context, request);
            LoggingUtility.LogIncomingRequest(incomingRequestLog);
            
            using (var buffer = new MemoryStream()) {
                HttpResponse response = context.Response;
                var bodyStream = response.Body;
                response.Body = buffer;
                await _next(context);
                CpRequest cpRequest = new CpRequest
                {
                    Method = request.Method
                };
                CpResponse cpResponse = new CpResponse
                {
                    StatusCode = response.StatusCode,
                    Body = new Body
                    {
                        Bytes = response.ContentLength ?? buffer.Length
                    }
                };
                // response time da calcolare con un timer?
                CompletedRequestLog completedRequestLog = BuildCompletedRequestLog(context, cpRequest, cpResponse, request);
                buffer.Position = 0;
                await buffer.CopyToAsync(bodyStream);
                LoggingUtility.LogCompletedRequest(completedRequestLog);
            }
        }

        private static CompletedRequestLog BuildCompletedRequestLog(HttpContext context, CpRequest cpRequest, CpResponse cpResponse, HttpRequest request)
        {
            return new CompletedRequestLog
            {
                Http = new Http
                {
                    Request = cpRequest,
                    Response = cpResponse
                },
                Url = new Url() {
                    Path = request.GetDisplayUrl(),
                },
                UserAgent = new UserAgent
                {
                    Original = request.Headers["User-Agent"].ToString()
                },
                Host = new Host()
                {
                    Hostname = request.Host.ToString(),
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
                ResponseTime = 0
            };
        }

        private static IncomingRequestLog BuildIncomingRequestLog(HttpContext context, HttpRequest request)
        {
            return new IncomingRequestLog
            {
                Http = new Http
                {
                    Request = new CpRequest
                    {
                        Method = request.Method
                    },
                    Response = null
                },
                Url = new Url
                {
                    Path = request.GetDisplayUrl()
                },
                UserAgent = new UserAgent
                {
                    Original = request.Headers["User-Agent"].ToString()
                },
                Host = new Host()
                {
                    Hostname = request.Host.ToString(),
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                }
            };
        }
    }
}