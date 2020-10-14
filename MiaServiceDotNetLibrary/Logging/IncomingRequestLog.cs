using MiaServiceDotNetLibrary.Logging.Entities;

namespace MiaServiceDotNetLibrary.Logging
{
    internal class IncomingRequestLog
    {
        public int Level { get; set; }
        public long Time { get; set; }
        public string ReqId { get; set; }
        public HttpIncoming Http { get; set; }
        public Url Url { get; set; }
        public Host Host { get; set; }
        public string Msg { get; set; }
    }
}