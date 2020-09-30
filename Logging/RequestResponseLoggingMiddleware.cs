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
            var cpRequest = new CpRequest
            {
                Method = request.Method,
                UserAgent = new UserAgent
                {
                    Original = request.Headers["User-Agent"].ToString()
                }
            };
            var reqId = GetReqId(request);
            var incomingRequestLog = BuildIncomingRequestLog(context, cpRequest, request, reqId);
            Logger.LogRequest(incomingRequestLog);
            
            using (var buffer = new MemoryStream()) {
                var bodyStream = response.Body;
                response.Body = buffer;
                await _next(context);
                responseStopwatch.Stop();
                var passedMicroSeconds = responseStopwatch.ElapsedMilliseconds / 1000m;
                var cpResponse = new CpResponse
                {
                    StatusCode = response.StatusCode,
                    Body = new Body
                    {
                        Bytes = response.ContentLength ?? buffer.Length
                    }
                };
                var completedRequestLog = BuildCompletedRequestLog(context, cpRequest, cpResponse, request, passedMicroSeconds, reqId);
                buffer.Position = 0;
                await buffer.CopyToAsync(bodyStream);
                Logger.LogRequest(completedRequestLog);
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

        private static IncomingRequestLog BuildIncomingRequestLog(HttpContext context, CpRequest cpRequest, HttpRequest request, long reqId)
        {
            return new IncomingRequestLog
            {
                Level = (int) LogLevels.Trace,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ReqId = reqId,
                Http = new HttpIncoming
                {
                    Request = cpRequest
                },
                Url = new Url
                {
                    Path = request.GetDisplayUrl()
                },
                Host = new Host
                {
                    Hostname = request.Host.ToString(),
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
            };
        }

        private static CompletedRequestLog BuildCompletedRequestLog(HttpContext context, CpRequest cpRequest, 
            CpResponse cpResponse, HttpRequest request, decimal responseTime, long reqId)
        {
            return new CompletedRequestLog
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