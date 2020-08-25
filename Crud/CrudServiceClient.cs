using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Crud
{
    public class CrudServiceClient
    {
        public string ApiPath { get; set; }
        public int CrudVersion { get; set; }
        public Dictionary<string, string> MiaHeaders { get; set; }
        
        private static readonly HttpClient Client;

        public CrudServiceClient(Dictionary<string, string> miaHeaders, string apiPath = "", int crudVersion = -1)
        {
            ApiPath = apiPath;
            CrudVersion = crudVersion;
            MiaHeaders = miaHeaders;
        }

        static CrudServiceClient()
        {
            Client = new HttpClient();
        }

        private void AddRequestHeaders(HttpRequestMessage message)
        {
            foreach (var (key, value) in  MiaHeaders)
            {
                message.Headers.Remove(key);
                message.Headers.Add(key, value);
            }
        }

        private Uri BuildUrl(string path, string queryString, string collectionName)
        {
            var baseUri = ApiPath;
            var suffix =
                CrudVersion != -1
                    ? $"/{CrudVersion.ToString()}/{collectionName}"
                    : $"/{collectionName}";
            return new Uri(baseUri + suffix);
        }

        private async Task<HttpResponseMessage> SendAsyncRequest(HttpMethod method, string path, string collectionName, string queryString,
            string body)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = BuildUrl(path, queryString, collectionName),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                AddRequestHeaders(httpRequestMessage);
                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
                // TODO log error
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<T>> RetrieveAll<T>()
        {
           var response = await SendAsyncRequest(HttpMethod.Get, ApiPath,  typeof(T).Name.ToLower(), "", "");
           var responseBody = await response.Content.ReadAsStringAsync();
           List<T> result = null;
           try
           {
               result = JsonSerializer.Deserialize<List<T>>(responseBody);
           }
           catch(Exception e)
           {
               //TODO log error
           }

           return result;
        }
    }
}