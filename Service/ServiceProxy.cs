using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceProxy
    {
        public string ServiceName { get; }
        public InitServiceOptions Options { get; }
        private static readonly HttpClient Client;

        static ServiceProxy()
        {
            Client = new HttpClient();
        }

        public static HttpRequestHeaders GetDefaultHeaders()
        {
            return Client.DefaultRequestHeaders;
        }

        public ServiceProxy(string serviceName, InitServiceOptions options)
        {
            ServiceName = serviceName;
            Options = options;
            if (options.Headers == null) return;
            foreach (var (key, value) in options.Headers)
            {
                Client.DefaultRequestHeaders.Add(key, value);
            }
        }

        private Uri BuildUrl(string path, string queryString, InitServiceOptions options)
        {
            var uriBuilder = new UriBuilder
            {
                Host = ServiceName,
                Path = path,
                Port = options?.Port ?? Options.Port,
                Scheme = (options?.Protocol ?? Options.Protocol).ToString(),
                Query = queryString
            };
            return uriBuilder.Uri;
        }

        private void AddRequestHeaders(HttpRequestMessage message, ServiceOptions options)
        {
            if (options == null) return;
            foreach (var (key, value) in options.Headers)
            {
                message.Headers.Add(key, value);
            }
        }

        public async Task<HttpResponseMessage> Get(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Post(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Put(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Patch(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Delete(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}