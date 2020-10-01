using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MiaServiceDotNetLibrary.Crud.library;
using MiaServiceDotNetLibrary.Crud.library.enums;

namespace MiaServiceDotNetLibrary.Crud
{
    public interface ICrudServiceClient
    {
        public Task<List<T>> Get<T>(Dictionary<string, string> query = null);
        public Task<T> GetById<T>(string id, Dictionary<string, string> query = null);
        public Task<int> Count<T>(Dictionary<string, string> query = null);
        public Task<HttpContent> Export<T>(Dictionary<string, string> query = null);
        public Task<HttpContent> Post<T>(T document, Dictionary<string, string> query = null);
        public Task<HttpContent> PostBulk<T>(List<T> documents, Dictionary<string, string> query = null);
        public Task<HttpStatusCode> PostValidate<T>(T document, Dictionary<string, string> query = null);
        public Task<HttpContent> UpsertOne<T>(T document, Dictionary<string, string> query = null);

        public Task<HttpContent> Patch<T>(Dictionary<PatchCodingKey, Dictionary<string, object>> body,
            Dictionary<string, string> query = null);

        public Task<HttpContent> PatchById<T>(string id, Dictionary<PatchCodingKey, Dictionary<string, object>> body,
            Dictionary<string, string> query = null);

        public Task<HttpContent> PatchBulk<T>(IList<PatchItemSection> body, Dictionary<string, string> query = null);
        public Task<HttpContent> Delete<T>(Dictionary<string, string> query = null);
        public Task<HttpContent> DeleteById<T>(string id, Dictionary<string, string> query = null);
    }
}
