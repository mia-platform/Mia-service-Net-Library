using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
        public Task<int> Count<T>();
        public Task<HttpContent> Export<T>();
        public Task<HttpContent> Post<T>(T document);
        public Task<HttpContent> PostBulk<T>(List<T> documents);
        public Task<HttpStatusCode> PostValidate<T>();
        public Task<HttpContent> UpsertOne<T>(T document);
        public Task<HttpContent> Patch<T>(JObject body);
        public Task<HttpContent> PatchById<T>(string id, JObject body);
        public Task<HttpContent> PatchBulk<T>(JArray body);
        public Task Delete<T>();
        public Task DeleteById<T>(string id);
    }
}