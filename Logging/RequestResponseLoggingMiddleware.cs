using System;
using System.Diagnostics;
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
        private long _reqIdAuto = 1;
        
        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            var responseStopwatch = new Stopwatch();
            responseStopwatch.Start();
            var request = context.Request;
            var response = context.Response;
            using (var buffer = new MemoryStream()) {
                var bodyStream = response.Body;
                response.Body = buffer;
                await _next(context);
                responseStopwatch.Stop();
                var passedMicroSeconds = responseStopwatch.ElapsedMilliseconds / 1000m;
                var cpRequest = new CpRequest
                {
                    method = request.Method
                };
                var cpResponse = new CpResponse
                {
                    statusCode = response.StatusCode,
                    body = new Body
                    {
                        bytes = response.ContentLength ?? buffer.Length
                    }
                };
                var reqId = GetReqId(request);
                var completedRequestLog = BuildCompletedRequestLog(context, cpRequest, cpResponse, request, passedMicroSeconds, reqId);
                buffer.Position = 0;
                await buffer.CopyToAsync(bodyStream);
                LoggingUtility.LogRequest(completedRequestLog);
            }
        }

        private long GetReqId(HttpRequest request)
        {
            long reqId;
            if (string.IsNullOrEmpty(request.Headers["x-request-id"]))
            {
                reqId = _reqIdAuto;
                _reqIdAuto++;
            }
            else
            {
                reqId = int.Parse(request.Headers["x-request-id"]);
                _reqIdAuto = 1;
            }
            return reqId;
        }

        private static CompletedRequestLog BuildCompletedRequestLog(HttpContext context, CpRequest cpRequest, 
            CpResponse cpResponse, HttpRequest request, decimal responseTime, long reqId)
        {
            return new CompletedRequestLog
            {
                level = LogLevels.Info,
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                reqId = reqId,
                http = new Http
                {
                    request = cpRequest,
                    response = cpResponse
                },
                url = new Url 
                {
                    path = request.GetDisplayUrl(),
                },
                userAgent = new UserAgent
                {
                    original = request.Headers["User-Agent"].ToString()
                },
                host = new Host
                {
                    hostname = request.Host.ToString(),
                    ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
                responseTime = responseTime
            };
        }
    }
}