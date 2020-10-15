using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using MiaServiceDotNetLibrary.Logging.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using log4net;

namespace MiaServiceDotNetLibrary.Logging
{
    public class MiddlewareOptions
    {
        public List<string> excludedPrefixes { get; set; } = new List<string>() {"/-/"};
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MiddlewareOptions _options;

        internal static readonly string  RequestIdDictionaryKey = "requestId";
        internal static readonly string  RequestIdHeaderKey = "x-request-id";
        internal static readonly string  UserAgentHeaderKey = "User-Agent";
        internal static readonly string  ForwardedHostHeaderKey = "x-forwarded-host";

        public RequestResponseLoggingMiddleware(RequestDelegate next, MiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }
        
        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path;
            foreach (string prefix in _options.excludedPrefixes)
            {
                if (path.StartsWith(prefix))
                {
                    await _next(context);
                    return;
                }
            }

            var responseStopwatch = new Stopwatch();
            responseStopwatch.Start();

            var request = context.Request;
            var response = context.Response;
            var serviceRequest = new ServiceRequest
            {
                Method = request.Method,
                UserAgent = new UserAgent
                {
                    Original = request.Headers[UserAgentHeaderKey].ToString()
                }
            };

            request.HttpContext.Items.Add(RequestIdDictionaryKey, GetOrGenerateReqId(request));

            var incomingRequestLog = BuildIncomingRequestLog(context, serviceRequest, request);
            Logger.LogRequest(incomingRequestLog);
            
            using (var buffer = new MemoryStream()) {
                var bodyStream = response.Body;
                response.Body = buffer;

                await _next(context);

                responseStopwatch.Stop();
                var passedMicroSeconds = responseStopwatch.ElapsedMilliseconds / 1000m;

                var serviceResponse = new ServiceResponse
                {
                    StatusCode = response.StatusCode,
                    Body = new Body
                    {
                        Bytes = response.ContentLength ?? buffer.Length
                    }
                };
                var completedRequestLog = BuildCompletedRequestLog(context, serviceRequest, serviceResponse, request, passedMicroSeconds);
                buffer.Position = 0;
                await buffer.CopyToAsync(bodyStream);

                Logger.LogRequest(completedRequestLog);
            }
        }

        internal static string GetOrGenerateReqId(HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.Headers[RequestIdHeaderKey]))
            {
                return (string)request.Headers[RequestIdHeaderKey];
            }

            if (!string.IsNullOrEmpty(request.HttpContext.TraceIdentifier))
            {
                return request.HttpContext.TraceIdentifier;
            }

            return Guid.NewGuid().ToString();
        }

        private static string RemovePort(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return "";
            }
            string[] HostComponents = host.Split(":");
            return HostComponents[0];
        }

        private static IncomingRequestLog BuildIncomingRequestLog(HttpContext context, ServiceRequest serviceRequest, HttpRequest request)
        {
            return new IncomingRequestLog
            {
                Level = (int) LogLevels.Trace,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ReqId = (string)request.HttpContext.Items[RequestIdDictionaryKey],
                Http = new HttpIncoming
                {
                    Request = serviceRequest
                },
                Url = new Url
                {
                    Path = request.Path,
                },
                Host = new Host
                {
                    Hostname = RemovePort(request.Host.ToString()),
                    ForwardedHostname = string.IsNullOrEmpty(request.Headers[ForwardedHostHeaderKey]) 
                    ? "" 
                    : (string)request.Headers[ForwardedHostHeaderKey],
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
                Msg = "incoming request"
            };
        }

        private static CompletedRequestLog BuildCompletedRequestLog(HttpContext context, ServiceRequest serviceRequest, 
            ServiceResponse serviceResponse, HttpRequest request, decimal responseTime)
        {
            return new CompletedRequestLog
            {
                Level = (int) LogLevels.Info,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ReqId = (string)request.HttpContext.Items[RequestIdDictionaryKey],
                Http = new Http
                {
                    Request = serviceRequest,
                    Response = serviceResponse
                },
                Url = new Url 
                {
                    Path = request.Path,
                },
                Host = new Host
                {
                    Hostname = RemovePort(request.Host.ToString()),
                    ForwardedHostname = string.IsNullOrEmpty(request.Headers[ForwardedHostHeaderKey]) 
                    ? "" 
                    : (string)request.Headers[ForwardedHostHeaderKey],
                    Ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString()
                },
                ResponseTime = responseTime,
                Msg = "request completed"
            };
        }
    }
}
