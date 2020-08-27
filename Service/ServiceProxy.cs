using System;
using System.Collections;
using System.Collections.Generic;
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
        public InitServiceOptions InitOptions { get; }
        private static readonly HttpClient Client;

        static ServiceProxy()
        {
            Client = new HttpClient();
        }


        public ServiceProxy(Dictionary<string, string> miaHeaders, string serviceName, InitServiceOptions initOptions)
        {
            ServiceName = serviceName;
            InitOptions = initOptions;
            
            AddMiaHeaders(miaHeaders);
        }

        private void AddMiaHeaders(Dictionary<string, string> miaHeaders)
        {
            foreach (var (key, value) in miaHeaders)
            {
                Client.DefaultRequestHeaders.Remove(key);
                Client.DefaultRequestHeaders.Add(key, value);
            }
        }

        private void AddRequestHeaders(HttpRequestMessage message, ServiceOptions options)
        {
            var mergedHeaders = GetMergedHeaders(options);
            foreach (var (key, value) in mergedHeaders)
            {
                message.Headers.Remove(key);
                message.Headers.Add(key, value);
            }
        }

        private Uri BuildUrl(string path, string queryString, InitServiceOptions options)
        {
            var uriBuilder = new UriBuilder
            {
                Host = ServiceName,
                Path = path,
                Port = options?.Port ?? InitOptions.Port,
                Scheme = (options?.Protocol ?? InitOptions.Protocol).ToString(),
                Query = queryString
            };
            return uriBuilder.Uri;
        }

        private Dictionary<string, string> GetMergedHeaders(ServiceOptions options)
        {
            var result = InitOptions.Headers ?? new Dictionary<string, string>();
            if (options == null) return result;
            foreach (var (key, value) in options.Headers)
            {
                result.Add(key, value);
            }

            return result;
        }

        private async Task<HttpResponseMessage> SendAsyncRequest(HttpMethod method, string path, string queryString,
            string body, ServiceOptions options)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = BuildUrl(path, queryString, options),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage, options);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                // TODO log error
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Get(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            return await SendAsyncRequest(HttpMethod.Get, path, queryString, body, options);
        }

        public async Task<HttpResponseMessage> Post(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            return await SendAsyncRequest(HttpMethod.Post, path, queryString, body, options);
        }

        public async Task<HttpResponseMessage> Put(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            return await SendAsyncRequest(HttpMethod.Put, path, queryString, body, options);
        }

        public async Task<HttpResponseMessage> Patch(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            return await SendAsyncRequest(HttpMethod.Patch, path, queryString, body, options);
        }

        public async Task<HttpResponseMessage> Delete(string path, string queryString = "", string body = "",
            ServiceOptions options = null)
        {
            return await SendAsyncRequest(HttpMethod.Delete, path, queryString, body, options);
        }
    }
}
