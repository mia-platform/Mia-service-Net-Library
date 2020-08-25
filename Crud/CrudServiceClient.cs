using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Crud
{
    public class CrudServiceClient
    {
        public string ApiPath { get; set; }
        public int CrudVersion { get; set; }
        public Dictionary<string, string> MiaHeaders { get; set; }

        private static readonly HttpClient Client;

        private const string VersionLiteral = "v";
        private const string BulkLiteral = "bulk";
        private const string ApiSecretHeaderKey = "secret";

        public CrudServiceClient(Dictionary<string, string> miaHeaders, string apiPath = default(string),
            string apiSecret = default(string), int crudVersion = default(int))
        {
            ApiPath = apiPath;
            CrudVersion = crudVersion;
            MiaHeaders = miaHeaders;

            if (apiSecret != default(string) && !Client.DefaultRequestHeaders.Contains(ApiSecretHeaderKey))
            {
                Client.DefaultRequestHeaders.Add(ApiSecretHeaderKey, apiSecret);
            }
        }

        static CrudServiceClient()
        {
            Client = new HttpClient();
        }

        private void AddRequestHeaders(HttpRequestMessage message)
        {
            foreach (var (key, value) in MiaHeaders)
            {
                message.Headers.Remove(key);
                message.Headers.Add(key, value);
            }
        }

        private string BuildPath(string collectionName)
        {
            var basePath = ApiPath;
            var suffix =
                CrudVersion != default(int)
                    ? $"/{VersionLiteral}{CrudVersion.ToString()}/{collectionName}"
                    : $"/{collectionName}";
            return basePath + suffix;
        }

        private Uri BuildUrl(string path)
        {
            return new Uri(path);
        }

        private static string GetCollectionName<T>()
        {
            return typeof(T).GetTypeInfo().GetCustomAttribute<JsonObjectAttribute>()?.Id;
        }

        private async Task<HttpResponseMessage> SendAsyncRequest(HttpMethod method, string path,
            string body)
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = BuildUrl(path),
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

        public async Task<List<T>> Get<T>()
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var response = await SendAsyncRequest(HttpMethod.Get, path, "");
            var responseBody = await response.Content.ReadAsStringAsync();
            List<T> result = null;
            try
            {
                result = JsonSerializer.Deserialize<List<T>>(responseBody);
            }
            catch (Exception e)
            {
                //TODO log error
            }

            return result;
        }

        public async Task<T> GetById<T>(string id)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var response = await SendAsyncRequest(HttpMethod.Get, path, "");
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = default(T);
            try
            {
                result = JsonSerializer.Deserialize<T>(responseBody);
            }
            catch (Exception e)
            {
                //TODO log error
            }

            return result;
        }

        public async Task<HttpContent> Post<T>(T document)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var body = JsonSerializer.Serialize(document);
            var response = await SendAsyncRequest(HttpMethod.Post, path, body);
            return response.Content;
        }
        
        public async Task<HttpContent> PostBulk<T>(List<T> documents)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{BulkLiteral}";
            var body = JsonSerializer.Serialize(documents);
            var response = await SendAsyncRequest(HttpMethod.Post, path, body);
            return response.Content;
        }
    }
}