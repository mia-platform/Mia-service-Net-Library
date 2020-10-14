using MiaServiceDotNetLibrary.Logging.Entities;

namespace MiaServiceDotNetLibrary.Logging
{
    internal class CompletedRequestLog
    {
        public int Level { get; set; }
        public long Time { get; set; }
        public string ReqId { get; set; }
        public Http Http { get; set; }
        public Url Url { get; set; }
        public Host Host { get; set; }
        public decimal ResponseTime { get; set; }
        public string Msg { get; set; }
    }
}
