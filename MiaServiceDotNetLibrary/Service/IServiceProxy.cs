using System.Net.Http;
using System.Threading.Tasks;

namespace Service
{
    public interface IServiceProxy
    {
        public Task<HttpResponseMessage> Get(string path, string queryString = "", string body = "",
            ServiceOptions options = null);

        public Task<HttpResponseMessage> Post(string path, string queryString = "", string body = "",
            ServiceOptions options = null);

        public Task<HttpResponseMessage> Put(string path, string queryString = "", string body = "",
            ServiceOptions options = null);

        public Task<HttpResponseMessage> Patch(string path, string queryString = "", string body = "",
            ServiceOptions options = null);

        public Task<HttpResponseMessage> Delete(string path, string queryString = "", string body = "",
            ServiceOptions options = null);
    }
}