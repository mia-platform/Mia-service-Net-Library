using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Service.Environment;

namespace Service
{
    public class ServiceProxy
    {
        public string ServiceName { get; }
        public HttpRequestHeaders Headers { get; }
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

        public ServiceProxy(string serviceName, InitServiceOptions options, HttpRequestHeaders miaHeaders)
        {
            ServiceName = serviceName;
            Headers = options.Headers;
            Options = options;
        }
        
        private Uri BuildUrl(string path, string queryString, InitServiceOptions options)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Host = ServiceName;
            uriBuilder.Path = path;
            uriBuilder.Port = options?.Port ?? Options.Port;
            uriBuilder.Scheme = (options?.Protocol ?? Options.Protocol).ToString();
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        public async Task<HttpResponseMessage> Get(string path, string queryString = "", ServiceOptions options = null)
        {
            try
            {
                var uri = BuildUrl(path, queryString, options);
                var response = await Client.GetAsync(uri);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Post(string path, string body, string queryString = "",
            ServiceOptions options = null)
        {
            try
            {
                var uri = BuildUrl(path, queryString, options);
                var response = await Client.PostAsync(uri,
                    new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json));
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Put(string path, string body, string queryString = "",
            ServiceOptions options = null)
        {
            try
            {
                var uri = BuildUrl(path, queryString, options);
                var response = await Client.PutAsync(uri,
                    new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json));
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Patch(string path, string body, string queryString = "",
            ServiceOptions options = null)
        {
            try
            {
                var uri = BuildUrl(path, queryString, options);
                var response = await Client.PatchAsync(uri,
                    new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json));
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<HttpResponseMessage> Delete(string path, string queryString = "",
            ServiceOptions options = null)
        {
            try
            {
                var uri = BuildUrl(path, queryString, options);
                var response = await Client.DeleteAsync(uri);
                return response;
            }
            catch (HttpRequestException e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}