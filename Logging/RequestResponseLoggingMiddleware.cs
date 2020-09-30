using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Logging.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private long _reqIdAuto = 1;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RequestResponseLoggingMiddleware));

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {   
            var loggingUtility = new Logger(_logger);
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
                    Method = request.Method,
                    UserAgent = new UserAgent
                    {
                        Original = request.Headers["User-Agent"].ToString()
                    }
                };
                var cpResponse = new CpResponse
                {
                    StatusCode = response.StatusCode,
                    Body = new Body
                    {
                        Bytes = response.ContentLength ?? buffer.Length
                    }
                };
                var reqId = GetReqId(request);
                var completedRequestLog = BuildCompletedRequestLog(context, cpRequest, cpResponse, request, passedMicroSeconds, reqId);
                buffer.Position = 0;
                await buffer.CopyToAsync(bodyStream);
                loggingUtility.LogRequest(completedRequestLog);
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

        private static RequestLog BuildCompletedRequestLog(HttpContext context, CpRequest cpRequest, 
            CpResponse cpResponse, HttpRequest request, decimal responseTime, long reqId)
        {
            return new RequestLog
            {
                Level = (int) LogLevels.Info,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ReqId = reqId,
                Http = new Http
                {
                    Request = cpRequest,
                    Response = cpResponse
                },
                Url = new Url 
                {
                    Path = request.GetDisplayUrl(),
                },
                Host = new Host
                {
                    Hostname = request.Host.ToString(),
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
                ResponseTime = responseTime
            };
        }
    }
}