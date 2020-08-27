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
        private const string ValidateLiteral = "validate";
        private const string UpsertOneLiteral = "upsert-one";
        private const string CountLiteral = "count";
        private const string ExportLiteral = "export";
        private const string ApiSecretHeaderKey = "secret";

        public CrudServiceClient(Dictionary<string, string> miaHeaders, string apiPath = default(string),
            string apiSecret = default(string), int crudVersion = default(int))
        {
            ApiPath = apiPath;
            CrudVersion = crudVersion;

            AddSecretHeader(apiSecret);
            AddMiaHeaders(miaHeaders);
        }

        private static void AddMiaHeaders(Dictionary<string, string> miaHeaders)
        {
            foreach (var (key, value) in miaHeaders)
            {
                Client.DefaultRequestHeaders.Remove(key);
                Client.DefaultRequestHeaders.Add(key, value);
            }
        }

        private static void AddSecretHeader(string apiSecret)
        {
            if (apiSecret != default(string) && !Client.DefaultRequestHeaders.Contains(ApiSecretHeaderKey))
            {
                Client.DefaultRequestHeaders.Add(ApiSecretHeaderKey, apiSecret);
            }
        }

        static CrudServiceClient()
        {
            Client = new HttpClient();
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

        public async Task<int> Count<T>()
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{CountLiteral}";
            var response = await SendAsyncRequest(HttpMethod.Get, path, "");
            var count = await response.Content.ReadAsStringAsync();
            var result = default(int);
            try
            {
                result = int.Parse(count);
            }
            catch (Exception e)
            {
                //TODO log error
            }

            return result;
        }
        
        public async Task<HttpContent> Export<T>()
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{ExportLiteral}";
            var response = await SendAsyncRequest(HttpMethod.Get, path, "");
            return response.Content;
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

        public async Task<HttpStatusCode> PostValidate<T>(T document)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{ValidateLiteral}";
            var response = await SendAsyncRequest(HttpMethod.Post, path, "");
            return response.StatusCode;
        }

        public async Task<HttpContent> UpsertOne<T>(T document)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{UpsertOneLiteral}";
            var body = JsonSerializer.Serialize(document);
            var response = await SendAsyncRequest(HttpMethod.Post, path, body);
            return response.Content;
        }

        public async Task<HttpContent> Patch<T>(JObject patchBody)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var body = patchBody.ToString();
            var response = await SendAsyncRequest(HttpMethod.Patch, path, body);
            return response.Content;
        }

        public async Task<HttpContent> PatchById<T>(string id, JObject patchBody)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var body = patchBody.ToString();
            var response = await SendAsyncRequest(HttpMethod.Patch, path, body);
            return response.Content;
        }

        public async Task<HttpContent> PatchBulk<T>(JArray patchBody)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{BulkLiteral}";
            var body = patchBody.ToString();
            var response = await SendAsyncRequest(HttpMethod.Patch, path, body);
            return response.Content;
        }
        
        public async Task<HttpContent> Delete<T>()
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var response = await SendAsyncRequest(HttpMethod.Delete, path, "");
            return response.Content;
        }

        public async Task<HttpContent> DeleteById<T>(string id)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var response = await SendAsyncRequest(HttpMethod.Delete, path, "");
            return response.Content;
        }
    }
}