using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
                CrudVersion != -1
                    ? $"/v{CrudVersion.ToString()}/{collectionName}"
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

        public async Task<List<T>> RetrieveAll<T>()
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

        public async Task<T> RetrieveById<T>(string id)
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
    }
}