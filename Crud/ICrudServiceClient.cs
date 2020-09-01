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
using Crud.library;
using Crud.library.enums;
using Newtonsoft.Json.Linq;

namespace Crud
{
    public interface ICrudServiceClient
    {
        public Task<List<T>> Get<T>(List<KeyValuePair<string, string>> query);
        public Task<T> GetById<T>(string id, List<KeyValuePair<string, string>> query);
        public Task<int> Count<T>(List<KeyValuePair<string, string>> query);
        public Task<HttpContent> Export<T>(List<KeyValuePair<string, string>> query);
        public Task<HttpContent> Post<T>(T document, List<KeyValuePair<string, string>> query);
        public Task<HttpContent> PostBulk<T>(List<T> documents, List<KeyValuePair<string, string>> query);
        public Task<HttpStatusCode> PostValidate<T>(T document, List<KeyValuePair<string, string>> query);
        public Task<HttpContent> UpsertOne<T>(T document, List<KeyValuePair<string, string>> query);

        public Task<HttpContent> Patch<T>(Dictionary<PatchCodingKey, Dictionary<string, JToken>> body,
            List<KeyValuePair<string, string>> query);

        public Task<HttpContent> PatchById<T>(string id, Dictionary<PatchCodingKey, Dictionary<string, JToken>> body,
            List<KeyValuePair<string, string>> query);

        public Task<HttpContent> PatchBulk<T>(PatchBulkBody body, List<KeyValuePair<string, string>> query);
        public Task<HttpContent> Delete<T>(List<KeyValuePair<string, string>> query);
        public Task<HttpContent> DeleteById<T>(string id, List<KeyValuePair<string, string>> query);
    }
}