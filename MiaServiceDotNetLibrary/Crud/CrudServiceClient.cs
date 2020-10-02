using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MiaServiceDotNetLibrary.Crud.library;
using MiaServiceDotNetLibrary.Crud.library.enums;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace MiaServiceDotNetLibrary.Crud
{
    public class CrudServiceClient : ICrudServiceClient
    {
        public string ApiPath { get; set; }
        public int CrudVersion { get; set; }
        public Dictionary<string, string> MiaHeaders { get; set; }

        private static readonly HttpClient Client;

        public Dictionary<string, string> DefaultQuery = new Dictionary<string, string>();

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

        private Uri BuildUrl(string path, string query)
        {
            var uriBuilder = new UriBuilder(path) {Query = query};
            return uriBuilder.Uri;
        }

        private string BuildQueryString(IReadOnlyCollection<KeyValuePair<string, string>> query)
        {
            return query == null
                ? new QueryBuilder(DefaultQuery).ToQueryString().ToString()
                : new QueryBuilder(DefaultQuery.Concat(query)).ToQueryString().ToString();
        }

        private static string GetCollectionName<T>()
        {
            return typeof(T).GetTypeInfo().GetCustomAttribute<CollectionName>()?.Value;
        }

        private async Task<HttpResponseMessage> SendAsyncRequest(HttpMethod method, string path,
            string queryString = "",
            string body = "")
        {
            try
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = method,
                    RequestUri = BuildUrl(path, queryString),
                    Content = new StringContent(body, Encoding.UTF8, MediaTypeNames.Application.Json)
                };

                var response = await Client.SendAsync(httpRequestMessage);
                return response;
            }
            catch (HttpRequestException e)
            {
               throw new CrudException($"HTTP request failed: {e.Message}");
            }
        }

        public async Task<List<T>> Get<T>(Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Get, path, queryString);
            var responseBody = await response.Content.ReadAsStringAsync();
            List<T> result = null;
            try
            {
                result = JsonConvert.DeserializeObject<List<T>>(responseBody);
            }
            catch (Exception e)
            {
                throw new CrudException($"Cannot read response body: {e.Message}");
            }

            return result;
        }

        public async Task<T> GetById<T>(string id, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Get, path, queryString);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = default(T);
            try
            {
                result = JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch (Exception e)
            {
                throw new CrudException($"Cannot read response body: {e.Message}");
            }

            return result;
        }

        public async Task<int> Count<T>(Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{CountLiteral}";
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Get, path, queryString);
            var count = await response.Content.ReadAsStringAsync();
            var result = default(int);
            try
            {
                result = int.Parse(count);
            }
            catch (Exception e)
            {
                throw new CrudException($"Cannot read response body: {e.Message}");
            }

            return result;
        }

        public async Task<HttpContent> Export<T>(Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{ExportLiteral}";
            var queryString = BuildQueryString(query);
            ;
            var response = await SendAsyncRequest(HttpMethod.Get, path, queryString);
            return response.Content;
        }

        public async Task<HttpContent> Post<T>(T document, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var body = JsonConvert.SerializeObject(document);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Post, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpContent> PostBulk<T>(List<T> documents, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{BulkLiteral}";
            var body = JsonConvert.SerializeObject(documents);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Post, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpStatusCode> PostValidate<T>(T document, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{ValidateLiteral}";
            var body = JsonConvert.SerializeObject(document);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Post, path, queryString, body);
            return response.StatusCode;
        }

        public async Task<HttpContent> UpsertOne<T>(T document, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{UpsertOneLiteral}";
            var body = JsonConvert.SerializeObject(document);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Post, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpContent> Patch<T>(Dictionary<PatchCodingKey, Dictionary<string, object>> patchBody,
            Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var body = JsonConvert.SerializeObject(patchBody);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Patch, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpContent> PatchById<T>(string id,
            Dictionary<PatchCodingKey, Dictionary<string, object>> patchBody,
            Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var body = JsonConvert.SerializeObject(patchBody);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Patch, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpContent> PatchBulk<T>(IList<PatchItemSection> patchBody,
            Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{BulkLiteral}";
            var body = JsonConvert.SerializeObject(patchBody);
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Patch, path, queryString, body);
            return response.Content;
        }

        public async Task<HttpContent> Delete<T>(Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/";
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Delete, path, queryString);
            return response.Content;
        }

        public async Task<HttpContent> DeleteById<T>(string id, Dictionary<string, string> query = null)
        {
            var path = $"{BuildPath(GetCollectionName<T>())}/{id}";
            var queryString = BuildQueryString(query);
            var response = await SendAsyncRequest(HttpMethod.Delete, path, queryString);
            return response.Content;
        }
    }
}
