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
        public Task<List<T>> Get<T>(string queryString = "");
        public Task<T> GetById<T>(string id, string queryString = "");
        public Task<int> Count<T>(string queryString = "");
        public Task<HttpContent> Export<T>(string queryString = "");
        public Task<HttpContent> Post<T>(T document, string queryString = "");
        public Task<HttpContent> PostBulk<T>(List<T> documents, string queryString = "");
        public Task<HttpStatusCode> PostValidate<T>(T document, string queryString = "");
        public Task<HttpContent> UpsertOne<T>(T document, string queryString = "");
        public Task<HttpContent> Patch<T>(PatchUpdateSection body, string queryString = "");
        public Task<HttpContent> PatchById<T>(string id, PatchUpdateSection body, string queryString = "");
        public Task<HttpContent> PatchBulk<T>(PatchBulkBody body, string queryString = "");
        public Task<HttpContent> Delete<T>(string queryString = "");
        public Task<HttpContent> DeleteById<T>(string id, string queryString = "");
    }
}