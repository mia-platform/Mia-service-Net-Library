using Logging.Entities;

namespace Logging
{
    public class RequestLog
    {
        public int Level { get; set; }
        public long Time { get; set; }
        public long ReqId { get; set; }
        public Http Http { get; set; }
        public Url Url { get; set; }
        public Host Host { get; set; }
        public decimal ResponseTime { get; set; }
    }
}