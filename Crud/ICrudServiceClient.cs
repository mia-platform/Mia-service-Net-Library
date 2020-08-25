using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Crud
{
    public interface ICrudServiceClient
    {
        public Task<List<T>> Get<T>();
        public Task<T> GetById<T>(string id);
        public Task<HttpContent> Post<T>(T document);
        public Task Delete<T>();
        public Task DeleteById<T>(string id);
        public Task<T> Patch<T>(JObject body);
        public Task<T> PatchById<T>(JObject body);
        public Task Export<T>();
        public Task<HttpStatusCode> PostValidate<T>();
        public Task<int> Count<T>();
        public Task<T> UpsertOne<T>(T document);
        public Task<HttpContent> PostBulk<T>(List<T> documents);
    }
}