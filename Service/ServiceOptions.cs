using System.Net.Http.Headers;

namespace Service
{
    public class ServiceOptions : InitServiceOptions
    {
        public ServiceOptions(int port = 3000, Protocol protocol = Protocol.HTTP, HttpRequestHeaders headers = null,
            string prefix = "") : base(port, protocol, headers, prefix)
        {
        }

        public ReturnAs ReturnAs { get; set; } = ReturnAs.JSON;
        public int[] AllowedStatusCodes { get; set; } = new int[] {200, 201, 202};
        public bool IsMiaHeaderInjected { get; set; } = true;
    }
}