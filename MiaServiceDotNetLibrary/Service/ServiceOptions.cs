using System.Collections.Generic;

namespace MiaServiceDotNetLibrary.Service
{
    public class  ServiceOptions : InitServiceOptions
    {
        public ServiceOptions(int port = 3000, Protocol protocol = Protocol.Http, Dictionary<string, string> headers = null,
            string prefix = "") : base(port, protocol, headers, prefix)
        {
        }

        public ReturnAs ReturnAs { get; set; } = ReturnAs.Json;
        public int[] AllowedStatusCodes { get; set; } = new int[] {200, 201, 202};
        public bool IsMiaHeaderInjected { get; set; } = true;
    }
}
