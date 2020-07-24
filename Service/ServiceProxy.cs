using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceProxy
    {
        public string ServiceName { get; }
        public HttpHeaders Headers { get; }
        public InitServiceOptions Options { get; }
        private static readonly HttpClient Client;

        static ServiceProxy()
        {
            Client = new HttpClient();
        }

        public ServiceProxy(string serviceName, InitServiceOptions options)
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
            if (queryString != null)
            {
                uriBuilder.Query = queryString;
            }

            return uriBuilder.Uri;
        }

        public async Task<HttpResponseMessage> Get(String path, String queryString, ServiceOptions options)
        {
            try
            {
                Uri uri = BuildUrl(path, queryString, options);

                HttpResponseMessage response = await Client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}